import express from 'express';
import bodyParser from 'body-parser';
import cors from 'cors';

export class App{
    public express: express.Application;

    constructor(){
        this.express = express();
        this.express.use(cors());
        this.express.use(bodyParser.urlencoded({extended:true}));
        this.express.use(bodyParser.json());
    }
}