from flask import Blueprint

main = Blueprint('main',__name__)

from safiricar.main import routes, forms, utils, models