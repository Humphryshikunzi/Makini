from base64 import b64encode
import platform
import requests

from flask import render_template, request, json, flash, redirect, session, url_for

from safiricar import app
from safiricar.config import BASE_URL, SEARCH_CRITERIA_OPTIONS
from safiricar.main.utils import get_headers


@app.route('/driver_status', methods=['GET', 'POST'])
def driver_status():
    '''
    get drivers here
    '''
    # If user not login then redirect to login page
    if not session.get('username'):
        return redirect(url_for('home'))

    formData = dict()

    roles = session['roles']
    role_infor = [role for role in roles if role['name'] == 'Driver']
    role_id = role_infor[0]['id']

    query = 'query { usersInRole(roleId:' + "\"{role_id}\"".format(role_id=role_id) + \
        ') {  id, email, userName, lastName, nationalId, phoneNumber, isActive, profilePictureDataUrl,\
              isVerified, ownedCars {licensePlate, id, color, model, imageUrl}}}'
    response = requests.post(
        BASE_URL, json={'query': query}, headers=get_headers(), verify=False)
    users_objs = json.loads(response.text)

    drivers = users_objs['data']['usersInRole']
    formData['driver_status'] = drivers
    session['drivers'] = drivers

    return render_template('driver/driver_status.html', formData=formData)


@app.route('/driver_profile', methods=['GET', 'POST'], defaults={'id': None})
@app.route('/driver_profile/<id>', methods=['GET', 'POST'])
def view_driver_profile(id):
    '''
    #View Driver Profile
    '''

    # If user not login then redirect to login page
    if not session.get('username'):
        return redirect(url_for('home'))

    global drivers
    formData = dict()

    try:
        drivers = session['drivers']
    except:
        roles = session['roles']
        role_infor = [role for role in roles if role['name'] == 'Driver']
        role_id = role_infor[0]['id']

        query = '''
        query {
        usersInRole(roleId: "%s") {
            id
            email
            userName
            lastName
            nationalId
            phoneNumber
            profilePictureDataUrl
            identificationDocument {
            nationalIdFront
            nationalIdRear
            id
            }
            ownedCars {
            licensePlate
            id
            color
            model
            imageUrl
            }
        }
        }

        ''' % (role_id)
        response = requests.post(
            BASE_URL, json={'query': query}, headers=get_headers(), verify=False)  
        
        users_objs = json.loads(response.text)

        drivers = users_objs['data']['usersInRole']
        formData['driver_status'] = drivers
        session['drivers'] = drivers

    if drivers:
        driver_obj = list(filter(lambda driver: driver['id'] == id, drivers))

        # get profile picture if any
        profile_url = driver_obj[0]['profilePictureDataUrl']
        if profile_url != '':
            t = 0
            system = platform.system()

            if (system == 'Linux'):
                t = profile_url.rfind('/')
            elif (system == 'Windows'):
                t = profile_url.rfind('\\')
                if (t == -1):
                    t = profile_url.rfind('/')

            file_name = profile_url[t+1:]

            profile_url_query = 'query {downloadFile(request: { fileName: ' + "\"{}\"".format(file_name) + \
                ', downloadType: PROFILE }) {fileName,data}}'
            image_profile_response = requests.post(
                BASE_URL, json={'query': profile_url_query}, headers=get_headers(), verify=False)
            user_profile_obj = json.loads(image_profile_response.text)

            try:
                profile_image_data = user_profile_obj['data']['downloadFile']['data']
                image_bytes = bytes(profile_image_data)
                image = b64encode(image_bytes).decode("utf-8")
                formData['profile_image'] = image
            except:
                formData['profile_image'] = None

        formData['driver'] = driver_obj[0]
        formData['update_id'] = driver_obj[0]['id']
        # get user activities, bad code now, needs to link userid and useractiviyid
        # user_activity_query = 'query {userActivities(where:{userId:{eq:' + "\"{}\"".format(driver_obj[0]['id']) +\
        #     '}}){id,title, description, userId, activityDate, timeOfActivity }}'
        # user_activity_response = requests.post(
        #     BASE_URL, json={'query': user_activity_query}, headers=get_headers(), verify=False)
        # user_activity_objs = json.loads(user_activity_response.text)

        # try:
        #     formData['userActivities_obj'] = user_activity_objs['data']['userActivities']
        # except:
        #     pass
    else:
        flash("Driver is either delete or  Driver id not valid")
        return redirect(url_for('driver_status'))

    return render_template('driver/driver_view_profile.html', formData=formData)


@app.route('/driver-search', methods=['GET', 'POST'])
def driver_search():
    '''
    search driver
    '''

    # If user not login then redirect to login page
    if not session.get('username'):
        return redirect(url_for('home'))

    # variable pass into template if any need
    formData = dict()
    formData['post_url'] = url_for('driver_search')

    if request.method == "POST":
        search_critera_key = request.form.get('search_criteria')
        search_critera = SEARCH_CRITERIA_OPTIONS[search_critera_key]
        search_value = request.form.get('search_value')
        query = 'query {users(where : { ' + "{search_critera}".format(search_critera=search_critera) + ':{eq:' +\
            "\"{search_value}\"".format(search_value=search_value) + '}}){email,lastName,id,isActive, nationalId, phoneNumber,\
                  userName,profilePictureDataUrl, isVerified, ownedCars{active, color, licensePlate, model}}}'
        response = requests.post(
            BASE_URL, json={'query': query}, headers=get_headers(), verify=False)
        users_objs = json.loads(response.text)

        try:
            drive_obj = users_objs['data']['users'][0]
            formData['driver'] = drive_obj
            formData['update_id'] = drive_obj['id']

            # get user activities, bad code now, needs to link userid and useractiviyid
            user_activity_query = 'query {userActivities(where:{userId:{eq:' + "\"{}\"".format(drive_obj['id']) + \
                '}}){id,title, description, userId, activityDate, timeOfActivity }}'
            user_activity_response = requests.post(
                BASE_URL, json={'query': user_activity_query}, headers=get_headers(), verify=False)
            user_activity_objs = json.loads(user_activity_response.text)

            try:
                formData['userActivities_obj'] = user_activity_objs['data']['userActivities']
            except:
                pass

            return render_template('driver/driver_view_profile.html', formData=formData)
        except:
            flash('This user does not exist or search criteria does not match')
            return render_template('driver/driver_search.html', formData=formData, search_criteria_options=SEARCH_CRITERIA_OPTIONS.keys())

    return render_template('driver/driver_search.html', formData=formData, search_criteria_options=SEARCH_CRITERIA_OPTIONS.keys())


@app.route('/ajax/delete_driver', methods=['POST'])
def ajax_delete_driver():
    '''
    Delete Driver details
    '''

    if not session.get('username'):
        data = {'url': url_for('home'), 'status': True}
        return data

    if request.method == "POST":
        driver_id = request.form.get('driver_id', 0)
        if driver_id:
            # Delete Driver here from db, disable account actually
            flash("Driver Details successfully deleted")
        else:
            flash("Something wents wrong, Please try again")

    data = {'status': True, 'url': url_for('driver_status')}

    return data


@app.route('/verify_driver_account/<id>', methods=['GET', 'POST'])
def verify_account(id):
    query = 'mutation {verifyUserAccount(userIdToVerifyAccount:' + \
        "\"{id}\"".format(id=id) + ') { data, messages, succeeded}}'
    response = requests.post(
        BASE_URL, json={'query': query}, headers=get_headers(), verify=False)
    user_obj = json.loads(response.text)
    activated_driver = user_obj['data']['verifyUserAccount']['succeeded']

    if bool(activated_driver):
        flash("Driver has been verified")
    else:
        flash("Error, Driver hasn't been verified")

    return redirect(url_for('view_driver_profile', id=id))


@app.route('/activate_driver_account/<id>', methods=['GET', 'POST'])
def activate_account(id):
    query = 'mutation {activateUserAccount(userIdToActivateAccount:' + \
        "\"{id}\"".format(id=id) + ') { data, messages, succeeded}}'
    response = requests.post(
        BASE_URL, json={'query': query}, headers=get_headers(), verify=False)
    user_obj = json.loads(response.text)
    activated_driver = user_obj['data']['activateUserAccount']['succeeded']

    if bool(activated_driver) == True:
        flash("Driver has been activated")
    else:
        flash("Error, Driver hasn't been activated")

    return redirect(url_for('view_driver_profile', id=id))


@app.route('/disable_driver_account/<id>', methods=['GET', 'POST'])
def disable_account(id):
    query = 'mutation {disableUserAccount(userIdToDisableAccount:' + \
        "\"{id}\"".format(id=id) + ') { data, messages, succeeded}}'
    response = requests.post(
        BASE_URL, json={'query': query}, headers=get_headers(), verify=False)
    user_obj = json.loads(response.text)
    disabled_driver = user_obj['data']['disableUserAccount']['succeeded']

    if bool(disabled_driver) == True:
        flash("Driver has been disabled")
    else:
        flash("Error, Driver hasn't been disabled")

    return redirect(url_for('view_driver_profile', id=id))


# export all drivers infor to excel
@app.route('/export_drivers_to_excel', methods=['GET', 'POST'])
def write_drivers_into_excel():
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
    role_infor = [role for role in roles if role['name'] == 'Driver']
    role_id = role_infor[0]['id']

    query = 'query { usersInRole(roleId:' + "\"{role_id}\"".format(role_id=role_id) + \
        ') { data { id, email, userName, lastName, nationalId, phoneNumber, isActive, profilePictureDataUrl, \
            ownedCars {licensePlate, id, color, model, imageUrl}}}}'
    response = requests.post(
        BASE_URL, json={'query': query}, headers=get_headers(), verify=False)
    users_objs = json.loads(response.text)

    drivers = users_objs['data']['usersInRole']
    dataframe = pd.DataFrame()
    iterate = 0
    for user in drivers:
        dataframe.at[iterate, "UserId"] = user['id']
        dataframe.at[iterate, "Email"] = user['email']
        # dataframe.at[iterate,"Date(YYYY-MM-DD)"] = user.created_at
        dataframe.at[iterate, "User Name"] = user['userName']
        dataframe.at[iterate, "National Id"] = user['nationalId']
        dataframe.at[iterate, "Phone Number"] = user['phoneNumber']
        dataframe.at[iterate, "Is Active"] = user['isActive']
        dataframe.at[iterate, "ProfileUrl"] = user['profilePictureDataUrl']
        dataframe.at[iterate, "No of Cars"] = len(user['ownedCars'])
        iterate += 1

    # create an output stream
    output = BytesIO()
    writer = pd.ExcelWriter(output, engine='xlsxwriter')

    # taken from the original question
    dataframe.to_excel(writer, startrow=0, merge_cells=False,
                       sheet_name="DriversSheet")
    workbook = writer.book
    worksheet = writer.sheets["DriversSheet"]
    format = workbook.add_format(
        {'border': 1, 'align': 'left', 'font_size': 10})
    format.set_bg_color('#eeeeee')
    # the writer has done its job
    writer.close()

    # go back to the beginning of the stream
    output.seek(0)

    # finally return the file
    return send_file(output, attachment_filename="drivers.xlsx", as_attachment=True)
