'use strict';

/** @type {import('sequelize-cli').Migration} */
module.exports = {
  async up (queryInterface, Sequelize) {
     return queryInterface.createTable('users', { 
        id:{
          type: Sequelize.INTEGER,
          primaryKey: true,
          autoIncrement: true,
          allowNull: false
        },
        name:{
          type: Sequelize.STRING,
          allowNull: false,
        },
        password:{
          type: Sequelize.STRING,
          allowNull: false
        },
        email:{
          type: Sequelize.STRING,
          allowNull: false,
          unique: true
        },
        picture:{
          type: Sequelize.BLOB,
          allowNull: true
        },
        created_at:{
          type: Sequelize.DATE,
          allowNull: false,
          defaultValue: Sequelize.NOW
        },
        updated_at:{
          type: Sequelize.DATE,
          allowNull: false,
          defaultValue: Sequelize.NOW
        }

    });
     
  },

  async down (queryInterface, Sequelize) {

    await queryInterface.dropTable('users');

  }
};
