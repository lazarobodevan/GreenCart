// const request = require('supertest');
 import sequelize from '..//database/index'
 import dbUtils from './utils/database'
 import request = require('supertest')
 import app from '../app'

describe("#USER",()=>{

    afterAll(async ()=>{
        await sequelize.query('SELECT * from Users')
        await dbUtils.clearUserTable();
        sequelize.close();
    })

    it('Should add a user', async ()=>{
        const res = await request(app).post('/users').send({
            name:'User test',
            email: 'test@test.com',
            password: '123'
        })

        console.log(res.body)
    })
})