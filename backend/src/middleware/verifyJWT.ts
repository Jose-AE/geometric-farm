import { Request, Response, NextFunction } from "express";
import jwt from "jsonwebtoken";
import config from "../config/config";

export interface TokenPayload {
  id: number;
  username: string;
  isAdmin: boolean;
  group: string;
}

export interface AuthenticatedRequest extends Request {
  user?: TokenPayload;
}

export function verifyToken(
  req: AuthenticatedRequest,
  res: Response,
  next: NextFunction
) {
  const authHeader = req.headers["authorization"];
  let token;
  if (authHeader) {
    token = authHeader.split(" ")[1];
  }

  if (!token) {
    return res.status(403).json({ message: "Token is required" });
  }

  jwt.verify(token, config.jwtSecret, (err, decoded) => {
    if (err) {
      return res.status(401).json({ message: "Failed to authenticate token" });
    }

    req.user = decoded as any;
    next();
  });
}
