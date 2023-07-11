import random
import string
 
#password_generator
def generate_password():   
    #parameters 
    pass_length = 12
    lower = string.ascii_lowercase
    upper = string.ascii_uppercase
    num = string.digits
    symbols = string.punctuation

    #combine parameters to generate password
    all = lower + upper + num + symbols
    temp = random.sample(all, pass_length)
    password = ''.join(temp)

    return password


