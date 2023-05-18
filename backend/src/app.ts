import express from 'express'
import router from './routes/index'

class App {
    public express: express.Application

    constructor(){
        this.express = express();
        this.middlewares();
        this.routes();
    }

    private middlewares():void{
        this.express.use(express.json())
    }

    private routes():void{
        this.express.get('/',(req,res)=>{
            return res.status(200).json({message:'Server is running'})
        })

        this.express.use('/', router());
    }
}

export default new App().express