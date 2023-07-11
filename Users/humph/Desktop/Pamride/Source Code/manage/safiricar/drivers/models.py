'''
from safiricar import db 
from sqlalchemy.dialects.mysql import TIMESTAMP
from sqlalchemy import text  

# Driver Model
class Driver(db.Model):
    __tablename__ = 'drivers'
    def __str__(self):
        return self.name
        
    # define attributes of customer table
    id = db.Column(db.Integer, primary_key=True)
    ssn_id = db.Column(db.Integer)    
    name = db.Column(db.String(255))
    age = db.Column(db.Integer)
    address = db.Column(db.String(255))
    state = db.Column(db.Integer)
    city = db.Column(db.Integer)
    created_at = db.Column(TIMESTAMP(), nullable=False)
    updated_at = db.Column(TIMESTAMP(), nullable=False,
                        server_default=text('CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP'),
                        )

    def __init__(self,ssn_id,name,age,address,country,city,created_at):        
        self.ssn_id = ssn_id
        self.name = name
        self.age = age
        self.address = address
        self.country= country
        self.city = city
        self.created_at = created_at 

# Driver Status
class DriverStatus(db.Model):
    __tablename__ = 'driver_status'
    def __str__(self):
        return "<Driver Id: "+self.name+">"
    id = db.Column(db.Integer,primary_key=True)
    driver_id = db.Column(db.Integer,db.ForeignKey('drivers.id'))
    ssn_id = db.Column(db.Integer)
    status = db.Column(db.Boolean,default=True)
    message = db.Column(db.String(255))
    updated_at = db.Column(TIMESTAMP(), nullable=False,
                        server_default=text('CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP'),
                        )
    def __init__(self,driver_id,ssn_id,message):
        self.driver_id = driver_id
        self.ssn_id = ssn_id
        self.message = message


# Country Model
class Country(db.Model):
    __tablename__ = 'countries'
    def __str__(self):
        return self.name
    
    # Columns
    id = db.Column(db.Integer,primary_key=True)
    sortname = db.Column(db.String(3))    
    name = db.Column(db.String(150))
    phonecode = db.Column(db.Integer)

# State Model
class State(db.Model):
    __tablename__ = 'states'
    def __str__(self):
        return self.name

    # Columns
    id = db.Column(db.Integer,primary_key=True)     
    name = db.Column(db.String(30))
    country_id = db.Column(db.Integer,db.ForeignKey('countries.id'))

# City Model
class City(db.Model):
    __tablename__ = 'cities'
    def __str__(self):
        return self.name
    
    # Columns
    id = db.Column(db.Integer,primary_key=True)     
    name = db.Column(db.String(30))
    state_id = db.Column(db.Integer,db.ForeignKey('states.id'))
'''    