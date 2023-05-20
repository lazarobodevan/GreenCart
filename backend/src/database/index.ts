import { Sequelize,  } from 'sequelize';
import User from '../models/User'
const dbConfig = require('./config/database')

class Database{

    public connection!:Sequelize;
    constructor(){
        console.log("Conectando ao banco...")
        this.loadDatabase();
        console.log("Conectado a "+process.env.NODE_ENV || 'dev')
    }

    private loadDatabase(){
        this.connect();
        this.initModels();
    }

    private connect(){
        this.connection = new Sequelize(dbConfig[process.env.NODE_ENV||'dev']);
    }

    private initModels(){
        User.init(this.connection);
    }
}




export default new Database().connection;