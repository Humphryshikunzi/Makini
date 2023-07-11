from flask import Flask 
from os import environ 
 
app = Flask(__name__)

app.secret_key = 'a very powerful and unique secrete key'
app.config['SQLALCHEMY_DATABASE_URI'] = environ.get('SQLALCHEMY_DATABASE_URI')
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
app.config['SALT'] = environ.get('SALT')
app.config["host"] = "localhost"
app.config["port"]= 9001 

from safiricar.drivers import drivers as drivers_blueprint
from safiricar.employees import employees as employees_blueprint
from safiricar.main import main as main_blueprint
from safiricar.passengers import passengers as passengers_blueprint
from safiricar.role_management import roles as roles_blueprint
 
app.register_blueprint(drivers_blueprint)
app.register_blueprint(employees_blueprint)
app.register_blueprint(main_blueprint)
app.register_blueprint(passengers_blueprint)
app.register_blueprint(roles_blueprint) 