"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var express_1 = __importDefault(require("express"));
var App = /** @class */ (function () {
    function App() {
        this.express = (0, express_1.default)();
        this.middlewares();
        this.routes();
    }
    App.prototype.middlewares = function () {
        this.express.use(express_1.default.json());
    };
    App.prototype.routes = function () {
        this.express.get('/', function (req, res) {
            return res.status(200).json({ message: 'Server is running' });
        });
    };
    return App;
}());
exports.default = new App().express;
