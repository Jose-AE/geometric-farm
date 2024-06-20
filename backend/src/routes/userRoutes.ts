import express from "express";
import User from "../models/user.model";
import jwt from "jsonwebtoken";

import {
  ICreateUserRequest,
  ILoginRequest,
  IUpdateUserRequest,
} from "../interfaces/userRequest.interfaces";
import config from "../config/config";
import {
  AuthenticatedRequest,
  TokenPayload,
  verifyToken,
} from "../middleware/verifyJWT";

const router = express.Router();

router.post("/login", async (req, res) => {
  const { password, username }: ILoginRequest = req.body;

  // Find user by username
  const user = await User.findOne({ where: { username, password } });

  if (!user) {
    return res.status(401).send("Invalid username or password");
  }

  const payload: TokenPayload = {
    id: (user as any).id,
    isAdmin: (user as any).is_admin,
    username: (user as any).username,
    group: (user as any).group,
  };

  // Generate JWT token
  const token = jwt.sign(payload, config.jwtSecret, {
    expiresIn: "720h",
  });

  res.json({ token });
});

router.put("/", verifyToken, async (req: AuthenticatedRequest, res) => {
  const { username, newData }: IUpdateUserRequest = req.body;

  if (req.user?.isAdmin == false)
    return res.status(400).send("You need to be admin to acces this route");

  const user = await User.findOne({
    where: { username: username },
  });

  if (!user) {
    return res.status(404).send(`No user found with username: ${username}`);
  }

  const data = newData;
  if (data.username) {
    (user as any).username = data.username;
  }
  if (data.isAdmin) {
    (user as any).is_admin = data.isAdmin;
  }
  if (data.password) {
    (user as any).password = data.password;
  }
  if (data.gorup) {
    (user as any).group = data.gorup;
  }

  await user.save();

  return res.send("User updated");
});

router.post("/", verifyToken, async (req: AuthenticatedRequest, res) => {
  if (req.user?.isAdmin == false)
    return res.status(401).send("You need to be admin to acces this route");

  const { isAdmin, password, username, group }: ICreateUserRequest = req.body;

  if (isAdmin == null || password == null || username == null || group == null)
    return res.status(400).send("Invalid request body");

  const foundUser = await User.findOne({ where: { username: username } });

  if (foundUser != null) return res.status(409).send("User already exists");

  try {
    await User.create({ is_admin: isAdmin, password, username, group } as any);
    return res.send("User created successfully");
  } catch (error) {
    return res.status(500).send({ error });
  }
});

router.delete("/", verifyToken, async (req: AuthenticatedRequest, res) => {
  if (req.user?.isAdmin == false)
    return res.status(401).send("You need to be admin to acces this route");

  const username = req.body.username;
  const foundUser = await User.findOne({ where: { username: username } });

  if (req.user?.username === username)
    return res.status(401).send("Cant delete self user");

  if (foundUser == null) {
    return res.status(400).send("User not found");
  } else {
    try {
      await User.destroy({
        where: { username: username },
      });
      return res.send("User deleted successfully");
    } catch (error) {
      return res.status(500).send({ error });
    }
  }
});

router.get("/", verifyToken, async (req: AuthenticatedRequest, res) => {
  if (req.user?.isAdmin == false)
    return res.status(401).send("You need to be admin to acces this route");

  const users = await User.findAll();

  return res.send(users);
});

export default router;
