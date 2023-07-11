'''
import hashlib  

from safiricar import app, db 
from sqlalchemy.dialects.mysql import TIMESTAMP
from sqlalchemy import text

# User Registration
class UserRegistration(db.Model):    
    __tablename__ = 'user_registrations' 
    # columns define
    id = db.Column(db.Integer,primary_key=True)
    username = db.Column(db.String(20))
    password = db.Column(db.String(255))
    status = db.Column(db.Boolean,default=True)
    created_at = db.Column(TIMESTAMP(), nullable=False)
    updated_at = db.Column(TIMESTAMP(), nullable=False,server_default=text('CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP'),)
    def __init__(self,username,password,created_at):
        self.username = username
        self.password = password
        self.created_at = created_at            

    def __str__(self):
        return self.username

    def hashing(password):
        salt = app.config.get('SALT','')    
        db_password = password+salt
        hash = hashlib.md5(db_password.encode())
        hashing_password = hash.hexdigest()             
        return hashing_password

'''