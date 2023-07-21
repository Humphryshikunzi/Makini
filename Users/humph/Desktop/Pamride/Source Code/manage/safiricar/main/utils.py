import json
import requests

from datetime import datetime, timezone
from flask import session 

from safiricar.config import BASE_URL

def get_formatted_date():
    now = datetime.now()
    formatted_date = now.strftime('%Y-%m-%d %H:%M:%S')
    return formatted_date

def get_roles():
    query =   "{\
                roles {\
                    data {\
                        id\
                        name\
                        description\
                    }\
                }\
            }"

    response = requests.post(BASE_URL, json={'query':query}, headers=get_headers(), verify=False) 
    roles_obj= json.loads(response.text)   

    try:
        if roles_obj:
            session['roles'] = roles_obj['data']['roles']['data'] 
            return True
    except:
        return False

def camel_case(s):
    return s[0].lower() + s[1:]

def get_headers ():
    token = session['token']
    refreshToken = session['refreshToken']
    stored_time = session['token_expiry_time']

    # Check if the token has expired
    token = check_token_expiration(token, refreshToken, stored_time)
    headers = {'Authorization': 'Bearer ' + token}
    
    return headers

def check_token_expiration(token, refreshToken, stored_time):
    current_time =  datetime.now(timezone.utc) 

    # Calculate the time difference between the current time and the stored time
    time_difference = stored_time - current_time 

    # Check if the token has expired (more than 1 hour)
    if time_difference.total_seconds() > 3600:
        # Retrieve a new token using the refreshToken and update the stored time
        token = get_new_token(refreshToken, token)  
        if token is None:
            raise ("There has been a problem retrieving a new token.")   
        return token
    else:
        # Token is still valid, return the existing token
        return token

def get_new_token(refresh_token, token):
    # Define the GraphQL mutation
    mutation = '''
        mutation GetAccessToken {
            accessToken(
                model: {
                    refreshToken: "%s"
                    token: "%s"
                }
            ) {
                data {
                    token
                    userName
                }
            }
        }
    ''' % (refresh_token, token)
 
    # Send the GraphQL mutation request
    response = requests.post(BASE_URL, json={'query': mutation})
    
    # Check if the request was successful
    if response.status_code == 200:
        # Extract the new token from the response
        new_token = response.json().get('data', {}).get('accessToken', {}).get('data', {}).get('token')
        session['token'] = new_token
        session['token_expiry_time'] = datetime.now(timezone.utc)
        return new_token
    else:
        print("Error:", response.text)
        return None


   