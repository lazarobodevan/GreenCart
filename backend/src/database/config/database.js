
module.exports={
    "dev":{
        "dialect": "postgres",
        "host":"localhost",
        "username": "postgres",
        "password": "123",
        "database": "postgres",
        "define":{
            "timestamps": true,
            "underscored": true
        }
    },
    "test":{
        "dialect": "postgres",
        "host":"localhost",
        "username": "postgres",
        "password": "123",
        "database": "test",
        "define":{
            "timestamps": true,
            "underscored": true
        }
    }
}