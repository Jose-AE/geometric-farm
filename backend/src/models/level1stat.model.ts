import { DataTypes, Model, ModelDefined, Optional } from "sequelize";
import { sequelize } from "../config/sequelize";
import Student from "./student.model";

interface Level1StatAttributes {
  id: number;
  student_id: number;
  selected_shape: string;
  expected_shape: string;
  is_correct: boolean;
}

type Level1StatCreationAttributes = Optional<Level1StatAttributes, "id">;

interface Level1StatModel
  extends ModelDefined<Level1StatAttributes, Level1StatCreationAttributes> {}

const Level1Stat: Level1StatModel = sequelize.define("level1_stat", {
  student_id: {
    type: DataTypes.INTEGER,
    allowNull: false,
  },
  selected_shape: {
    type: DataTypes.STRING,
    allowNull: false,
  },
  expected_shape: {
    type: DataTypes.STRING,
    allowNull: false,
  },
  is_correct: {
    type: DataTypes.BOOLEAN,
    allowNull: false,
  },
});

export default Level1Stat;
