import { IStudent } from "./sudent.interface";

export interface ILevelScore {
  id: number;
  student_id: number;
  level: number;
  score: number;
  createdAt: string;
  updatedAt: string;
  student: IStudent;
}
