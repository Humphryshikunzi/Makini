import requests   
import platform 

from flask import render_template, request, json, flash, redirect, session, url_for 
from base64 import b64encode 

from safiricar import app
from safiricar.config import BASE_URL, SEARCH_CRITERIA_OPTIONS, HEADERS
from safiricar.employees.utils import generate_password 
from safiricar.main.utils import get_headers

@app.route('/employee', methods=['GET','POST'], defaults={'update_id': None}) 
def employee_add(update_id):  
    """
     Employee Management, create an employee
    """
     # If user not login then redirect to login page
    if not session.get('username'):    
        return redirect(url_for('home')) 

    formData = dict()

    # pass update id 
    formData['update_id'] = update_id 

    # Post Request Handle
    if request.method == "POST": 
        postData = request.form     
        employee_first_name = postData['employee_first_name']            
        employee_last_name = postData['employee_last_name']
        employee_user_name = postData['employee_user_name']
        employee_national_id = postData['employee_national_id']
        employee_email = postData['employee_email']
        employee_phone_number = postData['employee_phone_number'] 
        employe_role ='Employee'
        employee_password =  generate_password()  
        employee_created_by = "Management Website"
        
        query = 'mutation { registerUser (request:{firstName:'+ "\"{}\"".format(employee_first_name) + \
            ',role:'+ "\"{}\"".format(employe_role) + ',lastName:'+ "\"{}\"".format(employee_last_name) + \
                ',createdBy:'+ "\"{}\"".format(employee_created_by) + ', \
                    activateUser:true, autoConfirmEmail:true, confirmPassword:'+ "\"{}\"".format(employee_password) + \
                        ',  password:'+ "\"{}\"".format(employee_password) + ',nationalId:'+ "\"{}\"".format(employee_national_id) + \
                            ', email:'+ "\"{}\"".format(employee_email) + ', phoneNumber: '+ "\"{}\"".format(employee_phone_number) +\
                                  ', userName:'+ "\"{}\"".format(employee_user_name) + '}){	messages, succeeded}}'             
        response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False)
        user_obj= json.loads(response.text)  
        response_message = user_obj['data']['registerUser']['messages'][0]  
        
        flash(str(response_message)) 

        if response_message=='Ok':
            return redirect(url_for('employee_infor'))

    return render_template('employee/employee_register.html', formData = formData)


@app.route('/employee_status',methods=['GET','POST'])
def employee_status():
    '''
     Get Drivers
    '''
    formData = dict() 

    # If user not login then redirect to login page
    if not session.get('username'):    
        return redirect(url_for('home'))

    roles = session['roles']
    role_infor = [role for role in roles if role['name']=='Employee']
    role_id = role_infor[0]['id']
    query = 'query { usersInRole(roleId:' + "\"{role_id}\"".format(role_id=role_id) + \
        ') { id, email, userName, lastName, nationalId, phoneNumber, isActive, profilePictureDataUrl }}'
    
    response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False) 
    users_objs= json.loads(response.text) 
    
    users = users_objs['data']['usersInRole']
    formData['employee_status'] =  users  
    session['employees'] = users
    
    return render_template('employee/employee_status.html', formData = formData)


@app.route('/employee_profile', methods=['GET','POST'],defaults={'id': None})
@app.route('/employee_profile/<id>',methods=['GET','POST'])
def view_employee_profile(id):    
    '''
     View Driver Profile
    '''
    # If user not login then redirect to login page
    if not session.get('username'):    
        return redirect(url_for('home'))  

    formData = dict()                   
    global employees

    try:
        employees= session['employees']  
    except:
        roles = session['roles']
        role_infor = [role for role in roles if role['name']=='Employee']
        role_id = role_infor[0]['id']
        query = 'query { usersInRole(roleId:' + "\"{role_id}\"".format(role_id=role_id) + \
            ') { id, email, userName, lastName, nationalId, phoneNumber, isActive, profilePictureDataUrl }  }'
        
        response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False) 
        users_objs= json.loads(response.text) 
        
        employees = users_objs['data']['usersInRole']
        formData['employee_status'] =  employees  
        session['employees'] = employees 

    if employees:  
        if id =='employee_id_placeholder':        
            username = session['username'] 
            employee_obj =  list(filter(lambda driver: driver['userName']==username, employees)) 
            id = employee_obj[0]['id']  

        employee_obj =  list(filter(lambda driver: driver['id']==id, employees))   

        # get profile picture if any       
        profile_url = employee_obj[0]['profilePictureDataUrl']
        if profile_url!='':  
            t= 0
            system = platform.system() 

            if(system=='Linux'):    
                t = profile_url.rfind('/')
            elif(system=='Windows'):
                t= profile_url.rfind('\\')

            file_name = profile_url[t+1:] 

            profile_url_query = 'query {downloadFile(request: { fileName: ' + "\"{}\"".format(file_name) + \
                ', downloadType: PROFILE_PICTURE }) {fileName,data}}'
            image_profile_response = requests.post(BASE_URL, json={'query': profile_url_query}, headers=get_headers(), verify=False)
            user_profile_obj = json.loads(image_profile_response.text)  
            profile_image_data = user_profile_obj['data']['downloadFile']['data']  
            image_bytes = bytes(profile_image_data)
            image = b64encode(image_bytes).decode("utf-8") 
          
            formData['profile_image'] =  image

        formData['employee'] =  employee_obj[0]             
        formData['update_id'] = employee_obj[0]['id']

        # get user activities, bad code now, needs to link userid and useractiviyid
        user_activity_query ='query {userActivities(where:{userId:{eq:' + "\"{}\"".format(employee_obj[0]['id']) +\
              '}}){id,title, description, userId, activityDate, timeOfActivity }}'
        user_activity_response = requests.post(BASE_URL, json={'query':user_activity_query}, headers=get_headers(), verify=False)
        user_activity_objs= json.loads(user_activity_response.text)   

        try:        
            formData['userActivities_obj'] = user_activity_objs['data']['userActivities']  
        except:
            pass
    else:
        flash("Employee is either deleted or  Employee id not valid")
        return redirect(url_for('employee_status'))

    return render_template('employee/employee_view_profile.html', formData = formData) 


@app.route('/employee-search',methods=['GET','POST'])
def employee_search():
    '''
    Search Driver 
    '''
    # variable pass into template if any need
    formData = dict()

    # If user not login then redirect to login page
    if not session.get('username'):
        return redirect(url_for('home'))

    # If user not login then redirect to login page
    if not session.get('username'):
        return redirect(url_for('home'))

    formData['post_url'] = url_for('employee_search')  

    if request.method == "POST":              
        search_critera_key = request.form.get('search_criteria')  
        search_critera =  SEARCH_CRITERIA_OPTIONS[search_critera_key] 
        search_value = request.form.get('search_value')  
        
        query ='query {users(where : { ' + "{search_critera}".format(search_critera=search_critera) + ':{eq:' + \
            "\"{search_value}\"".format(search_value=search_value) + '}}){email,lastName,id,isActive, nationalId, phoneNumber, userName}}'
        response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False)
        users_objs= json.loads(response.text)  

        try:
            employee_obj = users_objs['data']['users'][0]              
            formData['employee'] = employee_obj             
            formData['update_id'] = employee_obj['id']

            # get user activities, bad code now, needs to link userid and useractiviyid
            user_activity_query ='query {userActivities(where:{userId:{eq:' + "\"{}\"".format(employee_obj['id']) + \
                '}}){id,title, description, userId, activityDate, timeOfActivity }}'
            user_activity_response = requests.post(BASE_URL, json={'query':user_activity_query}, headers=get_headers(), verify=False)
            user_activity_objs= json.loads(user_activity_response.text)   


            try:        
                formData['userActivities_obj'] = user_activity_objs['data']['userActivities']  
            except:
                pass

            return render_template('employee/employee_view_profile.html',formData = formData)
        except:
            flash('This user does not exist or invalid search criteria')
            return render_template('employee/employee_search.html',formData = formData, search_criteria_options=SEARCH_CRITERIA_OPTIONS.keys())       
   
    return render_template('employee/employee_search.html',formData = formData, search_criteria_options=SEARCH_CRITERIA_OPTIONS.keys())   


@app.route('/ajax/delete_employee',methods=['POST'])
def ajax_delete_employee(): 
    '''
    Delete Driver details
    '''
    if not session.get('username'):    
        data = {'url' : url_for('home'),'status' : True}        
        return data

    if request.method == "POST":  
        driver_id = request.form.get('employee_id',0)
        if driver_id: 
            #Delete the Employee here in the db, disable account actually
            flash("Employee Details successfully deleted")
        else:
            flash("Something wents wrong, Please try again")
    data = {'status' : True,'url' : url_for('employee_status')}
    
    return data


@app.route('/disable_employee_account/<id>',methods=['GET','POST'])
def disable_employee_account(id): 
    query = 'mutation {disableUserAccount(userIdToDisableAccount:' +  "\"{id}\"".format(id=id) + ') { data, messages, succeeded}}'
    response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False) 
    user_obj= json.loads(response.text)  
    disabled_employee = user_obj['data']['disableUserAccount']['succeeded']
   
    if bool(disabled_employee) == True:
        flash("Employee has been disabled")
    else:
        flash("Error, Driver hasn't been disabled")
        
    return redirect(url_for('view_employee_profile', id=id))

@app.route('/add_to_role/<employee_id>')
def add_employee_to_role(employee_id): 
    '''
    Make a current employee with less previleges have more functionalities
    '''

    # ensure that only admins make others admins
    roles = session['roles']
    role_infor = [role for role in roles if role['name']=='Administrator']
    role_id = role_infor[0]['id']
    query = 'query { usersInRole(roleId:' + "\"{role_id}\"".format(role_id=role_id) + \
        ') { id, email, userName, lastName, nationalId, phoneNumber, isActive, profilePictureDataUrl }  }'
    
    response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False) 
    users_objs= json.loads(response.text) 
    users = users_objs['data']['usersInRole']

    username = session['username']
    is_user_admin = [user for user in users if users if user['userName']== username] 

    if len(is_user_admin)==0:
        flash("Only Admins can make other Employees Admins")
        return redirect(url_for('view_employee_profile', id=employee_id))

    role_name = "Administrator" # hardcoded for now
    query = 'mutation{ addUserToRole(userId:'+ "\"{employee_id}\"".format(employee_id=employee_id) +\
          ', roleName:' + "\"{role_name}\"".format(role_name=role_name) + '){messages, succeeded}}'
    response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False) 
    user_obj= json.loads(response.text)  
    employee_added_to_role = user_obj['data']['addUserToRole']['succeeded']
   
    if bool(employee_added_to_role) == True:
        flash("Employee has been added to role: " + role_name )
    else:
        flash("Error, Employee hasn't been added to the role:" + role_name)
        
    return redirect(url_for('view_employee_profile', id=employee_id))


@app.route('/employee_profile/upload_profile/<id>', methods=['POST'])
def upload_profile(id):   
    '''
    This function updates the user profile image 
    '''  
    file_item = request.files['filename']  
     
    # check if the file has been uploaded
    if file_item.filename:

        # strip the leading path from the file name 
        data_bytes = file_item.read()
        data_array = list(data_bytes) 
        data_len = len(data_array)  

        if data_len > 5097152:
            raise('Maximum file size is 2MB')

        file_name = file_item.filename.split('.')[0]
        file_extension = file_item.filename.split('.')[1]
        query = 'mutation {	saveUploadeFile(request: {fileName:' + f"\"{file_name}\"," + 'extension:' +\
              f"\"{file_extension}\"," + f'uploadType: PROFILE_PICTURE, data:{data_array}' + '})}'
        
        response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False) 
       
        print(response.content)

        # check request status for future, for flashing messages

    return redirect(url_for('view_employee_profile', id=id)) 


@app.route('/export_employees_to_excel',methods=['GET','POST'])
def write_employees_into_excel():
    '''
    This function exports all employees infor to excel
    '''
    
    if not session.get('username'):
        return redirect(url_for('home')) 
          
    formData = dict()         

    import numpy as np
    import pandas as pd
    from io import BytesIO
    from flask import send_file

    roles = session['roles']
    role_infor = [role for role in roles if role['name']=='Employee']
    role_id = role_infor[0]['id']
    query = 'query { usersInRole(roleId:' + "\"{role_id}\"".format(role_id=role_id) + \
        ') { id, email, userName, lastName, nationalId, phoneNumber, isActive, profilePictureDataUrl }}'
    response = requests.post(BASE_URL, json={'query':query}, headers= get_headers(), verify=False) 
    users_objs= json.loads(response.text) 
    
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
    dataframe.to_excel(writer, startrow = 0, merge_cells = False, sheet_name = "EmployeesSheet")
    workbook = writer.book
    worksheet = writer.sheets["EmployeesSheet"]
    format = workbook.add_format({'border':1,'align':'left','font_size':10})
    format.set_bg_color('#eeeeee')    
    #the writer has done its job
    writer.close()

    #go back to the beginning of the stream
    output.seek(0)
 
    #finally return the file
    return send_file(output, attachment_filename="employees.xlsx", as_attachment=True) 



