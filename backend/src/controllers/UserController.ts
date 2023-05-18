import { Request, Response } from 'express';
import User from '../models/User';


class UserController{

    public async createUser(req: Request, res:Response):Promise<Response>{
        const {name, email, password, picture} = req.body
        const user = await User.create({name, email, password});
        return res.json(user);
    }

}

export default new UserController();