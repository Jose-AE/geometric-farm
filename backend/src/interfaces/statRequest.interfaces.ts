export interface IAddLevel1StatRequest {
  studentListNum: number;
  studentGroup: string;
  selectedShape: string;
  expectedShape: string;
  isCorrect: boolean;
}

export interface IAddLevel2StatRequest {
  studentListNum: number;
  studentGroup: string;
  operationType: string;
  isCorrect: boolean;
}

export interface IAddLevel3StatRequest {
  studentListNum: number;
  studentGroup: string;
  shapeType: string;
  operationType: string;
  isCorrect: boolean;
}

export interface IAddLevelScoreRequest {
  studentListNum: number;
  studentGroup: string;
  score: number;
  level: number;
}

export interface ICreateStudentRequest {
  gender: string;
  group: string;
  listNumber: number;
}

export interface IVerifyStudentRequest {
  group: string;
  listNumber: number;
}
