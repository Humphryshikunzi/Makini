'''
from sqlalchemy.dialects.mysql import TIMESTAMP
from sqlalchemy import text,DDL,event  

from safiricar import db



# Customer Model
class Customer(db.Model):
    __tablename__ = 'customers'
    def __str__(self):
        return self.name
        
    # define attributes of customer table
    id = db.Column(db.Integer,primary_key=True)
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

    def __init__(self,ssn_id,name,age,address,state,city,created_at):        
        self.ssn_id = ssn_id
        self.name = name
        self.age = age
        self.address = address
        self.state = state
        self.city = city
        self.created_at = created_at        


# Customer Status
class CustomerStatus(db.Model):
    __tablename__ = 'customer_status'
    def __str__(self):
        return "<Customer Id: "+self.name+">"
    id = db.Column(db.Integer,primary_key=True)
    customer_id = db.Column(db.Integer,db.ForeignKey('customers.id'))
    ssn_id = db.Column(db.Integer)
    status = db.Column(db.Boolean,default=True)
    message = db.Column(db.String(255))
    updated_at = db.Column(TIMESTAMP(), nullable=False,
                        server_default=text('CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP'),
                        )
    def __init__(self,customer_id,ssn_id,message):
        self.driver_id = customer_id
        self.ssn_id = ssn_id
        self.message = message


event.listen(
            Customer.__table__,
            "after_create",
            DDL("ALTER TABLE `%(table)s` auto_increment = 900000000;")
        )  


'''        