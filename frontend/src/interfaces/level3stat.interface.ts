import { IStudent } from "./sudent.interface";

export interface ILevel3Stat {
  id: number;
  student_id: number;
  operation_type: "area" | "perimiter";
  shape_type: string;
  is_correct: boolean;
  createdAt: string;
  updatedAt: string;
  student: IStudent;
}
