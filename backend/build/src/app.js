"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const index_1 = __importDefault(require("./routes/index"));
class App {
    constructor() {
        this.express = (0, express_1.default)();
        this.middlewares();
        this.routes();
    }
    middlewares() {
        this.express.use(express_1.default.json());
    }
    routes() {
        this.express.get('/', (req, res) => {
            return res.status(200).json({ message: 'Server is running' });
        });
        this.express.use('/', (0, index_1.default)());
    }
}
exports.default = new App().express;
