import { PrismaClient } from "@prisma/client";
import logger from "../logger";

class Database{
    public connection!: PrismaClient;

    constructor(){
        this.connect();
    }

    private connect(){
        try{
            logger.info("Connecting to the database");
            this.connection = new PrismaClient();
            logger.info("Connected to the database");
        }catch(e){
            logger.error(e);
        }
    }
}

export default new Database().connection;