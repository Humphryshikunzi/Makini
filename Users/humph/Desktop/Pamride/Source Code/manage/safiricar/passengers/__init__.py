from flask import Blueprint

passengers = Blueprint('passengers',__name__)

from safiricar.passengers import routes, forms, utils, models