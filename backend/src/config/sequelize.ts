import { Sequelize } from "sequelize";
import config from "./config";

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
    })
    .catch((err) => {
      console.log(err);
    });
}
