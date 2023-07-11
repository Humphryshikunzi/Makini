from flask import Blueprint

drivers = Blueprint('drivers',__name__)

from safiricar.drivers import routes, utils, forms, models