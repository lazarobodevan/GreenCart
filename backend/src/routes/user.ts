import  express  from "express";
import UserController from '../controllers/UserController'

export default(router:express.Router) =>{
    router.post('/users', UserController.createUser)
}