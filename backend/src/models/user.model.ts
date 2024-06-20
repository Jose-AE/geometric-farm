import { DataTypes, Model, ModelDefined, Optional } from "sequelize";
import { sequelize } from "../config/sequelize";

interface UserAttributes {
  id: number;
  username: string;
  password: string;
  is_admin: boolean;
  group: string;
}

type UserCreationAttributes = Optional<UserAttributes, "id">;

interface UserModel
  extends ModelDefined<UserAttributes, UserCreationAttributes> {}

const User: UserModel = sequelize.define("user", {
  username: {
    type: DataTypes.STRING,
    allowNull: false,
  },
  password: {
    type: DataTypes.STRING,
    allowNull: false,
  },
  group: {
    type: DataTypes.STRING,
    allowNull: true,
  },
  is_admin: {
    type: DataTypes.BOOLEAN,
    allowNull: false,
  },
});

export default User;
