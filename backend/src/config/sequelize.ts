import { Sequelize } from "sequelize";
import config from "./config";
import User from "../models/user.model";

export const sequelize = new Sequelize(
  config.mysql.database,
  config.mysql.user,
  config.mysql.password,
  {
    dialectModule: require("mysql2"),
    host: config.mysql.host,
    dialect: "mysql",
    port: config.mysql.port,
  }
);

export async function initSequelize() {
  await sequelize
    .authenticate()
    .then(() => {
      console.log("Connection to mySQL db has been established successfully.");
    })
    .catch((error) => {
      console.error("Unable to connect to the database: ", error);
    });

  //sync models
  await sequelize
    .sync()
    .then((result) => {
      console.log("DB models synced");
      ensureBaseUser();
    })
    .catch((err) => {
      console.log(err);
    });
}




async function ensureBaseUser() {
  const baseUsername = 'admin';
  const basePassword = 'password1234';
  try {
    const user = await User.findAll();

    if (user.length === 0) {
      await User.create({
        is_admin: true,
        group: "",
        password: basePassword,
        username: baseUsername
      });
      console.log('Base user created successfully.');
    }
  } catch (error) {
    console.error('Error ensuring base user:', error);
  }
}
