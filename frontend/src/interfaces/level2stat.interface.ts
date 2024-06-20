import { IStudent } from "./sudent.interface";

export interface ILevel2Stat {
  id: number;
  student_id: number;
  operation_type: "+" | "-" | "*";
  is_correct: boolean;
  createdAt: string;
  updatedAt: string;
  student: IStudent;
}
