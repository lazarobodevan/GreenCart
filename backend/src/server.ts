import { App } from "./app";
import dotenv from 'dotenv';
import logger from "./logger";

class Server{
    private app: App;

    constructor(){
        dotenv.config();
        this.app = new App();
        this.initServer();
    }

    private initServer(){
        this.app.express.listen(process.env.SERVER_PORT);
        logger.info(`Listening on port ${process.env.SERVER_PORT}`);
    }
}

const server = new Server();