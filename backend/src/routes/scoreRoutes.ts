import express from "express";
import { IAddLevelScoreRequest } from "../interfaces/scoreRequests.interface";
import LevelScore from "../models/level_score.model";
import Student from "../models/student.model";
import { AuthenticatedRequest, verifyToken } from "../middleware/verifyJWT";

const router = express.Router();

router.post("/", async (req, res) => {
  const { level, score, studentGroup, studentListNum }: IAddLevelScoreRequest =
    req.body;

  const student = await Student.findOne({
    where: {
      list_number: studentListNum,
      group: studentGroup as string,
    },
  });

  if (!student)
    return res.status(400).send("No student with that list number and group");

  try {
    await LevelScore.create({
      level: level,
      score: score,
      student_id: (student as any).id,
    } as any);
    res.send("Score created successfully");
  } catch (error) {
    res.status(500).send({ error });
  }
});

router.get("/", verifyToken, async (req: AuthenticatedRequest, res) => {
  const levelQuery = req.query.level;
  let level;

  if (levelQuery) {
    level = parseInt(req.query.level as string);
    if (Number.isNaN(level))
      return res.status(400).send("Level must be a number");
  }

  if (req.user?.isAdmin) {
    try {
      const stats = await LevelScore.findAll({
        where: levelQuery ? { level } : {},
        include: [{ model: Student }],
      });

      return res.send({ stats });
    } catch (error) {
      return res.status(500).send({ error });
    }
  } else {
    try {
      const stats = await LevelScore.findAll({
        where: levelQuery ? { level } : {},
        include: [{ model: Student, where: { group: req.user?.group } }],
      });
      return res.send({ stats });
    } catch (error) {
      return res.status(500).send({ error });
    }
  }
});

export default router;
