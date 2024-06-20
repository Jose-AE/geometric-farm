import { DataTypes, Model, ModelDefined, Optional } from "sequelize";
import { sequelize } from "../config/sequelize";
import Student from "./student.model";

interface Level2StatAttributes {
  id: number;
  student_id: number;
  operation_type: string;
  is_correct: boolean;
}

type Level2StatCreationAttributes = Optional<Level2StatAttributes, "id">;

interface Level2StatModel
  extends ModelDefined<Level2StatAttributes, Level2StatCreationAttributes> {}

const Level2Stat: Level2StatModel = sequelize.define("level2_stat", {
  student_id: {
    type: DataTypes.INTEGER,
    allowNull: false,
  },
  operation_type: {
    type: DataTypes.STRING,
    allowNull: false,
  },
  is_correct: {
    type: DataTypes.BOOLEAN,
    allowNull: false,
  },
});

export default Level2Stat;
