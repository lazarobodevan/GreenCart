"use strict";
const sequelize_1 = require("sequelize");
class User extends sequelize_1.Model {
    static init(connection) {
        super.init({
            name: sequelize_1.DataTypes.STRING,
            email: sequelize_1.DataTypes.STRING,
            password: sequelize_1.DataTypes.STRING,
            picture: sequelize_1.DataTypes.BLOB
        }, {
            sequelize: connection
        });
    }
}
module.exports = User;
