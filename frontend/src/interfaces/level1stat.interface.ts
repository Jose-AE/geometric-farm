import { IStudent } from "./sudent.interface";

export interface ILevel1Stat {
  id: number;
  student_id: number;
  selected_shape: string;
  expected_shape: string;
  is_correct: boolean;
  createdAt: string; // Or Date if you plan to work with Date objects
  updatedAt: string; // Or Date if you plan to work with Date objects
  student: IStudent;
}
