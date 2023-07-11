import requests

from flask import render_template, json, redirect, session, url_for 

from safiricar import app
from safiricar.config import BASE_URL
from safiricar.main.utils import get_headers 

 #Create Role
@app.route('/create_role', methods=['GET','POST'], defaults={'update_id': None}) 
def create_role(update_id): 

    formData = dict()

    # If user not login then redirect to login page
    if not session.get('username'):    
        return redirect(url_for('home')) 

    # pass update id 
    formData['update_id'] = update_id  

    '''
    # Post Request Handle
    if request.method == "POST": 
        postData = request.form     
        role_name = postData['role_name']            
        role_description = postData['role_description'] 
        query = 'mutation { registerUser (request:{firstName:'+ "\"{}\"".format(role_name) + ',lastName:'+ "\"{}\"".format(role_description) + '}){	messages, succeeded}}'             
        response = requests.post(BASE_URL, json={'query':query}, verify=False)
        user_obj= json.loads(response.text)  
        response_message = user_obj['data']['registerUser']['messages'][0]  
        flash(str(response_message)) 
        if response_message=='Ok':
            return redirect(url_for('get_roles'))

    '''
    return render_template('role_management/register_role.html', formData = formData)


#Get Roles
@app.route('/get_roles',methods=['GET','POST'])
def get_roles():

    formData = dict() 

    # If user not login then redirect to login page
    if not session.get('username'):    
        return redirect(url_for('home'))

    query = """{\
            roles {\
                data {\
                    id\
                    name\
                    description\
                }\
            }\
        }""" 

    response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False) 
    users_objs= json.loads(response.text) 
    users = users_objs['data']['roles']['data']
    formData['roles'] =  users  

    return render_template('role_management/view_roles.html', formData = formData)
 
