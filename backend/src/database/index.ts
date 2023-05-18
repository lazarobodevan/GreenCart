import { Sequelize,  } from 'sequelize';
import User from '../models/User'
const dbConfig = require('./config/database')

console.log("Conectando ao banco...")
const connection = new Sequelize(dbConfig);
console.log("Conectado")
User.init(connection);


export = connection;