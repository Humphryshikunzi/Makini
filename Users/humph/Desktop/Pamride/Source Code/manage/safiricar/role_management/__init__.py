from flask import Blueprint

roles = Blueprint('roles',__name__)

from safiricar.role_management import routes, utils, forms, models