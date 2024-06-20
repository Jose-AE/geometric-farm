import { DataTypes, Model, ModelDefined, Optional } from "sequelize";
import { sequelize } from "../config/sequelize";
import Student from "./student.model";

interface Level3StatAttributes {
  id: number;
  student_id: number;
  operation_type: string;
  shape_type: string;
  is_correct: boolean;
}

type Level3StatCreationAttributes = Optional<Level3StatAttributes, "id">;

interface Level3StatModel
  extends ModelDefined<Level3StatAttributes, Level3StatCreationAttributes> {}

const Level3Stat: Level3StatModel = sequelize.define("level3_stat", {
  student_id: {
    type: DataTypes.INTEGER,
    allowNull: false,
  },
  operation_type: {
    type: DataTypes.STRING,
    allowNull: false,
  },
  shape_type: {
    type: DataTypes.STRING,
    allowNull: false,
  },
  is_correct: {
    type: DataTypes.BOOLEAN,
    allowNull: false,
  },
});

export default Level3Stat;
