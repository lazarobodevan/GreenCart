"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const sequelize_1 = require("sequelize");
const User_1 = __importDefault(require("../models/User"));
const dbConfig = require('./config/database');
class Database {
    constructor() {
        console.log("Conectando ao banco...");
        this.loadDatabase();
        console.log("Conectado a " + process.env.NODE_ENV || 'dev');
    }
    loadDatabase() {
        this.connect();
        this.initModels();
    }
    connect() {
        this.connection = new sequelize_1.Sequelize(dbConfig[process.env.NODE_ENV || 'dev']);
    }
    initModels() {
        User_1.default.init(this.connection);
    }
}
exports.default = new Database().connection;
