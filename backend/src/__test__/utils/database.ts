import sequelize from '../../database/index'

const clearUserTable = async() =>{
    await sequelize.query('TRUNCATE USERS');
}

export = {
    clearUserTable
}
