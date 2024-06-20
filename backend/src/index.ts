import express, { Express, Request, Response } from "express";
import userRouter from "./routes/userRoutes";
import studentRouter from "./routes/studentRoutes";
import statsRouter from "./routes/statsRoutes";
import scoresRouter from "./routes/scoreRoutes";

import cors from "cors";
import { initSequelize } from "./config/sequelize";
import config from "./config/config";

const app: Express = express();

const corsOptions = {
  origin: [config.webappUrl, "http://localhost:5173"],
  methods: ["GET", "POST", "PUT", "DELETE"],
  allowedHeaders: ["Content-Type", "Authorization"],
};

app.use(cors(corsOptions));
app.use(express.json());
app.use("/user", userRouter);
app.use("/student", studentRouter);
app.use("/stats", statsRouter);
app.use("/scores", scoresRouter);

app.get("/", (req: Request, res: Response) => {
  res.send("Geometric Farm API v1");
});

app.get("/doc", (req: Request, res: Response) => {
  res.sendFile(__dirname + "/doc/index.html");
});

app.listen(config.port, async () => {
  await initSequelize();
  console.log(`[SERVER]: Server is running at http://localhost:${config.port}`);
});

export default app;
