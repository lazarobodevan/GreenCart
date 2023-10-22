import winston from 'winston';

class Logger{
    public logger: winston.Logger;

    constructor(){
        this.logger = winston.createLogger({
            transports:[new winston.transports.Console()]
        });
    }
}

export default new Logger().logger;