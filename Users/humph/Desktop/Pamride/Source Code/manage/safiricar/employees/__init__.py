import imp
from flask import Blueprint

employees = Blueprint('employees',__name__)

from safiricar.employees import routes, forms, utils, models