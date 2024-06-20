import { DataTypes, Model, ModelDefined, Optional } from "sequelize";
import { sequelize } from "../config/sequelize";
import Level1Stat from "./level1stat.model";
import Level2Stat from "./level2stat.model";
import Level3Stat from "./level3stat.model";
import LevelScore from "./level_score.model";

interface StudentAttributes {
  id: number;
  gender: string;
  group: string;
  list_number: number;
}

type StudentCreationAttributes = Optional<StudentAttributes, "id">;

interface StudentModel
  extends ModelDefined<StudentAttributes, StudentCreationAttributes> {}

const Student: StudentModel = sequelize.define("student", {
  gender: {
    type: DataTypes.CHAR,
    allowNull: false,
  },
  group: {
    type: DataTypes.STRING,
    allowNull: false,
  },
  list_number: {
    type: DataTypes.INTEGER,
    allowNull: false,
  },
});

Student.hasMany(Level1Stat, {
  foreignKey: "student_id",
  onDelete: "CASCADE",
});

Level1Stat.belongsTo(Student, {
  foreignKey: "student_id",
});

Student.hasMany(Level2Stat, {
  foreignKey: "student_id",
  onDelete: "CASCADE",
});

Level2Stat.belongsTo(Student, {
  foreignKey: "student_id",
});

Student.hasMany(Level3Stat, {
  foreignKey: "student_id",
  onDelete: "CASCADE",
});

Level3Stat.belongsTo(Student, {
  foreignKey: "student_id",
});

Student.hasMany(LevelScore, {
  foreignKey: "student_id",
  onDelete: "CASCADE",
});

LevelScore.belongsTo(Student, {
  foreignKey: "student_id",
});

export default Student;
