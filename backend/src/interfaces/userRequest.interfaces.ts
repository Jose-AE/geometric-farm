export interface ICreateUserRequest {
  username: string;
  password: string;
  isAdmin: boolean;
  group: string;
}

export interface IUpdateUserRequest {
  username: string;
  newData: {
    username: null | string;
    password: null | string;
    isAdmin: null | boolean;
    gorup: string;
  };
}

export interface IDeleteUserRequest {
  username: string;
}

export interface ILoginRequest {
  password: string;
  username: string;
}
