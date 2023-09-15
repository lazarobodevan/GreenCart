import { App } from "./app";
import dotenv from 'dotenv';

class Server{
    private app: App;

    constructor(){
        dotenv.config();
        this.app = new App();
        this.initServer();
    }

    private initServer(){
        this.app.express.listen(process.env.SERVER_PORT);
        console.log(`Listening on port ${process.env.SERVER_PORT}`);
    }
}

const server = new Server();