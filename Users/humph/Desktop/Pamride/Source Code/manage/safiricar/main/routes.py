import datetime
import requests

from flask import render_template, request, json, flash, redirect, session, url_for

from safiricar import app
from safiricar.config import BASE_URL 
from safiricar.main.utils import get_headers, get_roles


@app.route('/', methods=['GET','POST'])
def home():
    '''
    main app entry, login here
    '''

    # If user already login then redirect to customer search page
    if session.get('username'):    
        return redirect(url_for('customer_search'))
    
    # Allow Login Functionality
    formData = dict()  
        
    if request.method == "POST":          
        email = request.form.get('email',None) 
        password = request.form.get('password',None)  

        formData['email'] =  email
        formData['password'] = password
        
        if email and password: 
            # Check username exist or not 
            query = 'mutation {	login( request: { password:' + "\"{password}\"".format(password=password)  + \
            ', email:'+ "\"{email}\"".format(email=email) + '}) {succeeded,	messages, data {refreshToken,\
                  refreshTokenExpiryTime, token, userName, userImageURL}}}'
            response = requests.post(BASE_URL, json={'query':query}, verify=False)   
            user_obj= json.loads(response.text)   

            try:
                if user_obj:
                    user_name = user_obj['data']['login']['data']['userName'] 
                    token = user_obj['data']['login']['data']['token']
                    refreshToken = user_obj['data']['login']['data']['refreshToken']
                    profilePictureDataUrl = user_obj['data']['login']['data']['userImageURL']
                    
                    session['token'] = token
                    session['refreshToken'] = refreshToken  
                    session['profilePictureDataUrl'] = profilePictureDataUrl 
                    session["token_expiry_time"] = datetime.datetime.now() 
                    
                    # get roles and store in session 
                    get_roles() 
                                       
                    global employees

                    roles = session['roles']
                    role_infor = [role for role in roles if role['name']=='Employee']
                    role_id = role_infor[0]['id']
                    query = 'query { usersInRole(roleId:' + "\"{role_id}\"".format(role_id=role_id) + ') { id, email, userName, lastName, nationalId, phoneNumber, isActive, profilePictureDataUrl } }'
                    

                    response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False)  
                    users_objs= json.loads(response.text) 
                    
                    employees = users_objs['data']['usersInRole']
                    formData['employee_status'] =  employees  
                    session['employees'] = employees 
                    
                    employee_obj =  list(filter(lambda employee: employee['userName']==user_name, employees)) 

                    if(len(employee_obj)==0):
                        flash('Only Employees for Pamride are allowed to access this content')  
                        return redirect(url_for('home'))
                    
                    session['username'] = user_name

                    flash("User login successfully")
                    return redirect(url_for('customer_search'))
                else:
                    flash("Username or password invalid")
            except:              
                flash('Seems this user is not registered')  
                return redirect(url_for('home'))
        else:
            flash("Something wents wrong, please contact developer")

    return render_template('index.html', formData = formData)

'''
@app.route('/create-account',methods=['GET','POST'])
def create_account():
    formData = dict()  

    if request.method == "POST":
        username = request.form.get('username',None) 
        password = request.form.get('password',None)         
        formData['username'] = username
        formData['password'] = password

        if username and password:
            # Check username exist or not
            user_exists = UserRegistration.query.filter_by(username=username).count()   

            if not user_exists:
                # perform operation
                encrypt_password = UserRegistration.hashing(password)
                user_obj = UserRegistration(username,encrypt_password,get_formatted_date())
                # saved object into db
                db.session.add(user_obj)  # Adds new User record to database            
                db.session.commit()  # Commits all changes
                flash("User created successfully")

                return redirect(url_for('home'))
            else:
                flash("Username is already present please choose another")
        else:
            flash("Something wents wrong, please contact developer")

    return render_template('registration.html',formData = formData)


'''

@app.route('/logout',methods=['GET','POST'])
def logout():
    '''
    logout by clearing cache 
    '''
    if session.get('username'): 
        session.clear()

    return redirect(url_for('home'))
    

@app.route('/contact',methods=['GET','POST'])
def contact(): 
    '''
    for contact page
    '''
    return render_template('main/contact.html')    


@app.route('/users_groups',methods=['GET','POST'])
def users_groups(): 
    '''
    for dashboard page
    '''
    passengers = 20000
    drivers = 2000
    employees = 200

    users_by_region = {
        'Task' : 'Pamride Users By Regions', 
        'Nairobi' : 10000,
        'Kisumu' : 3000,
        'Mombasa' : 4000,
        'Nakuru' : 2000,
        'Nyeri' : 1000,
    }
    
    users_by_type = {
        'Task' : 'Pamride Users By Type', 
        'Passengers: {}'.format(passengers) : passengers, 
        'Drivers: {}'.format(drivers) : drivers, 
        'Full Time Employees: {}'.format(employees) : employees
    } 
	
    return render_template('main/users_groups.html', users_by_type=users_by_type, users_by_region=users_by_region)    

@app.route('/revenue_groups',methods=['GET','POST'])
def revenue_groups(): 
    '''
    for dashboard page
    '''
    passengers = 20000
    drivers = 2000
    employees = 200

    revenue_by_Quarter = {
        'Task' : 'Revenue By Quarter', 
        'Q1' : 2000000,
        'Q2' : 3000000,
        'Q3' : 4000000,
        'Q4' : 3500000, 
    }
    
    revenue_share = {
        'Task' : 'Statistics', 
        'Drivers Total:' : 10625000, 
        'Pamride Profit:' : 1850000,  
    } 
	
    return render_template('main/revenue_groups.html', revenue_share=revenue_share, revenue_by_Quarter=revenue_by_Quarter)    