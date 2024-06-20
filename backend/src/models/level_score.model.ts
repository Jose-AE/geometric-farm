import { DataTypes, Model, ModelDefined, Optional } from "sequelize";
import { sequelize } from "../config/sequelize";
import Student from "./student.model";

interface LevelScoreAttributes {
  id: number;
  student_id: number;
  score: number;
  level: number;
}

type LevelScoreCreationAttributes = Optional<LevelScoreAttributes, "id">;

interface LevelScoreModel
  extends ModelDefined<LevelScoreAttributes, LevelScoreCreationAttributes> {}

const LevelScore: LevelScoreModel = sequelize.define("level_score", {
  student_id: {
    type: DataTypes.INTEGER,
    allowNull: false,
  },
  level: {
    type: DataTypes.INTEGER,
    allowNull: false,
  },
  score: {
    type: DataTypes.INTEGER,
    allowNull: false,
  },
});

export default LevelScore;
