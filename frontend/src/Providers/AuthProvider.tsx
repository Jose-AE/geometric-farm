import axios from "axios";
import React, { createContext, useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";

interface IUserJWT {
  id: number;
  isAdmin: boolean;
  username: string;
  group: string;
  iat: number;
  exp: number;
}

interface IUser {
  username: string;
  group: string;
  isAdmin: boolean;
}

interface IAuthContext {
  user: IUser | null;
  token: string | null;
  login: (username: string, password: string) => Promise<boolean>;
  logout: () => void;
  isLoggedIn: () => boolean;
}

export const AuthContext = createContext<IAuthContext>({} as IAuthContext);

interface AuthProviderProps {
  children: React.ReactNode;
}

export default function AuthProvider({ children }: AuthProviderProps) {
  const [token, setToken] = useState<string | null>(null);
  const [user, setUser] = useState<IUser | null>(null);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const user = localStorage.getItem("user");
    const token = localStorage.getItem("token");
    if (user && token) {
      setUser(JSON.parse(user));
      setToken(token);
    }
    setIsReady(true);
    axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
  }, []);

  function isLoggedIn() {
    return user ? true : false;
  }

  async function login(username: string, password: string): Promise<boolean> {
    try {
      const response = await axios.post(
        `${import.meta.env.VITE_API_URL || "http://localhost:3000"}/user/login`,
        {
          password,
          username,
        }
      );
      const token = response.data.token;
      setToken(token);

      localStorage.setItem("token", token);

      const userJwtObject: IUserJWT = jwtDecode(token);

      const userObj: IUser = {
        isAdmin: userJwtObject.isAdmin,
        username: userJwtObject.username,
        group: userJwtObject.group,
      };

      setUser(userObj);
      localStorage.setItem("user", JSON.stringify(userObj));

      return true;
    } catch (err) {
      console.log(err);
      return false;
    }
  }

  async function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");

    setUser(null);
    setToken(null);
  }

  return (
    <AuthContext.Provider value={{ user, isLoggedIn, login, logout, token }}>
      {isReady ? children : null}
    </AuthContext.Provider>
  );
}
