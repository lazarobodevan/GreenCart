import {Model, DataTypes, ModelStatic} from 'sequelize';

class User extends Model{
    static init(connection:any):any{
        super.init({
            name: DataTypes.STRING,
            email: DataTypes.STRING,
            password: DataTypes.STRING,
            picture: DataTypes.BLOB
        },{
            sequelize:connection
        })
    }
}

export = User