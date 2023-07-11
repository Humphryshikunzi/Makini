import platform
import requests

from base64 import b64encode
from flask import render_template, request, json, flash, redirect, session, url_for 

from safiricar import app 
from safiricar.config import BASE_URL, SEARCH_CRITERIA_OPTIONS 
from safiricar.main.utils import get_headers
 
@app.route('/customer_status',methods=['GET','POST'])
def customer_status():
    '''
     get the list of passengers
    '''
   # If user not login then redirect to login page
    if not session.get('username'):    
        return redirect(url_for('home'))

    formData = dict()  

    roles = session['roles']
    role_infor = [role for role in roles if role['name']=='Passenger']
    role_id = role_infor[0]['id']
    
    query = 'query { usersInRole(roleId:' + "\"{role_id}\"".format(role_id=role_id) + ') \
        { id, email, userName, lastName, nationalId, phoneNumber, isActive, profilePictureDataUrl }  }'
    response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False)
    print(response.status_code)
    users_objs = json.loads(response.text) 
   
    users = users_objs['data']['usersInRole']
    formData['customer_status'] =  users 
    session['users'] = users
   
    return render_template('passenger/customer_status.html', formData = formData)     
  

@app.route('/profile', methods=['GET','POST'],defaults={'id': None})
@app.route('/profile/<id>', methods=['GET','POST'])
def view_profile(id):   
    '''
    retrieve information about a certain passenger given id and display
    '''
    # If user not login then redirect to login page
    if not session.get('username'):            
        return redirect(url_for('home'))    
    
    formData = dict()  
    global users 
    
    try:
        users = session['users']
    except:
        roles = session['roles']
        role_infor = [role for role in roles if role['name']=='Passenger']
        role_id = role_infor[0]['id']

        query = 'query { usersInRole(roleId:' + "\"{role_id}\"".format(role_id=role_id) + ') \
            { id, email, userName, lastName, nationalId, phoneNumber, isActive, profilePictureDataUrl }}'
        response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False)
        users_objs = json.loads(response.text) 
    
        users = users_objs['data']['usersInRole']
        formData['customer_status'] =  users 
        session['users'] = users
   
    if users:  
        customer_obj =  list(filter(lambda driver: driver['id']==id, users))  

         # get profile picture if any       
        profile_url = customer_obj[0]['profilePictureDataUrl']
        if profile_url!='':  
            t= 0
            system = platform.system() 

            if(system=='Linux'):    
                t = profile_url.rfind('/')
            elif(system=='Windows'):
                t= profile_url.rfind('\\')

            file_name = profile_url[t+1:] 

            profile_url_query = 'query {downloadFile(request: { fileName: ' +\
                  "\"{}\"".format(file_name) + ', downloadType: PROFILE_PICTURE }) {fileName,data}}'
            image_profile_response = requests.post(BASE_URL, json={'query': profile_url_query}, headers=get_headers(), verify=False)
            user_profile_obj = json.loads(image_profile_response.text)  
            
            profile_image_data = user_profile_obj['data']['downloadFile']['data']  
            image_bytes = bytes(profile_image_data)
            image = b64encode(image_bytes).decode("utf-8")          
            formData['profile_image'] =  image

        formData['customer'] =  customer_obj[0]             
        formData['update_id'] = customer_obj[0]['id']

        # get user activities, bad code now, needs to link userid and useractiviyid
        user_activity_query ='query {userActivities(where:{userId:{eq:' + "\"{}\"".format(customer_obj[0]['id']) +\
              '}}){id,title, description, userId, activityDate, timeOfActivity }}'
        user_activity_response = requests.post(BASE_URL, json={'query':user_activity_query}, headers=get_headers(), verify=False)
        user_activity_objs= json.loads(user_activity_response.text)   

        try:        
            formData['userActivities_obj'] = user_activity_objs['data']['userActivities']  
        except:
            pass
    else:
        flash("Customer is either deleted or customer id not valid")     
        return redirect(url_for('customer_status'))
    
    return render_template('passenger/customer_view_profile.html', formData = formData)


@app.route('/customer-search',methods=['GET','POST'])
def customer_search():  
    '''
    search a passenger
    '''  
    # Customer Search 
    # variable pass into template if any need
    formData = dict()

    # If user not login then redirect to login page
    if not session.get('username'):
        return redirect(url_for('home'))

    formData['post_url'] = url_for('customer_search')       

    if request.method == "POST":               
        search_critera_key = request.form.get('search_criteria')  
        search_critera =  SEARCH_CRITERIA_OPTIONS[search_critera_key] 
        search_value = request.form.get('search_value')   
        
        query ='query {users(where : { ' + "{search_critera}".format(search_critera=search_critera) +\
              ':{eq:' + "\"{search_value}\"".format(search_value=search_value) + \
                '}}){email,lastName,id,isActive, nationalId, phoneNumber, userName, profilePictureDataUrl}}'
        response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False)
        users_objs= json.loads(response.text)  

        try:
            customer_obj = users_objs['data']['users'][0]              
            formData['customer'] = customer_obj             
            formData['update_id'] = customer_obj['id']

            # get user activities, bad code now, needs to link userid and useractiviyid
            user_activity_query ='query {userActivities(where:{userId:{eq:' + \
                "\"{}\"".format(customer_obj['id']) + '}}){id,title, description, userId, activityDate, timeOfActivity }}'
            user_activity_response = requests.post(BASE_URL, json={'query':user_activity_query}, headers=get_headers(), verify=False)
            user_activity_objs= json.loads(user_activity_response.text)   

            try:        
                formData['userActivities_obj'] = user_activity_objs['data']['userActivities']  
            except:
                pass

            return render_template('passenger/customer_view_profile.html',formData = formData)
        except:
            flash('This user does not exist or criteria does not match')
            return render_template('passenger/customer_search.html',formData = formData, search_criteria_options=SEARCH_CRITERIA_OPTIONS.keys())       
 
    return render_template('passenger/customer_search.html',formData = formData, search_criteria_options=SEARCH_CRITERIA_OPTIONS.keys())            


@app.route('/ajax/delete_customer',methods=['POST'])
def ajax_delete_customer():
    '''
    delete a customer
    '''
    if not session.get('username'):    
        data = {'url' : url_for('home'),'status' : True}  
        return data

    if request.method == "POST":  
        customer_id = request.form.get('customer_id',0)
        if customer_id:
            # delete customer here
            flash("Customer Details successfully deleted")
        else:
            flash("Something wents wrong, Please try again")

    data = {'status' : True,'url' : url_for('customer_status')}

    return data


@app.route('/disable_customer_account/<id>',methods=['GET','POST'])
def disable_customer_account(id): 
    query = 'mutation {disableUserAccount(userIdToDisableAccount:' +  "\"{id}\"".format(id=id) + ') { data, messages, succeeded}}'
    response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False) 
    user_obj= json.loads(response.text)  
    disabled_passenger = user_obj['data']['disableUserAccount']['succeeded']
   
    if bool(disabled_passenger) == True:
        flash("Passenger has been disabled")
    else:
        flash("Error, Driver hasn't been disabled")
        
    return redirect(url_for('view_profile', id=id))

@app.route('/export_passengers_to_excel', methods= ['GET','POST'])
def export_passengers_to_excel():
    '''
    export passengers information to excel
    '''
    if not session.get('username'):
        return redirect(url_for('home')) 
          
    formData = dict()         

    import numpy as np
    import pandas as pd
    from io import BytesIO
    from flask import send_file

    roles = session['roles']
    role_infor = [role for role in roles if role['name']=='Passenger']
    role_id = role_infor[0]['id']
   
    query = 'query { usersInRole(roleId:' + "\"{role_id}\"".format(role_id=role_id) + ') { id, email, userName, lastName, nationalId, phoneNumber, isActive, profilePictureDataUrl }  }'
    response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False)
    users_objs = json.loads(response.text) 

    users = users_objs['data']['usersInRole']
    dataframe = pd.DataFrame()        
    iterate = 0 

    for user in users:
        dataframe.at[iterate,"UserId"] = user['id']
        dataframe.at[iterate,"Email"] = user['email']
        # dataframe.at[iterate,"Date(YYYY-MM-DD)"] = user.created_at
        dataframe.at[iterate,"User Name"] = user['userName']
        dataframe.at[iterate,"National Id"] = user['nationalId']
        dataframe.at[iterate,"Phone Number"] = user['phoneNumber']
        dataframe.at[iterate,"Is Active"] = user['isActive']
        dataframe.at[iterate, "ProfileUrl"] = user['profilePictureDataUrl']
        iterate+=1

    #create an output stream
    output = BytesIO()
    writer = pd.ExcelWriter(output, engine='xlsxwriter')

    #taken from the original question
    dataframe.to_excel(writer, startrow = 0, merge_cells = False, sheet_name = "PassengersSheet")
    workbook = writer.book
    worksheet = writer.sheets["PassengersSheet"]
    format = workbook.add_format({'border':1,'align':'left','font_size':10})
    format.set_bg_color('#eeeeee')    
    #the writer has done its job
    writer.close()

    #go back to the beginning of the stream
    output.seek(0) 
    
    #finally return the file
    return send_file(output, attachment_filename="passengers.xlsx", as_attachment=True) 



