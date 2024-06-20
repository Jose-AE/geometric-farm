import express from "express";
import {
  IAddLevel1StatRequest,
  IAddLevel2StatRequest,
  IAddLevel3StatRequest,
} from "../interfaces/statRequest.interfaces";
import Student from "../models/student.model";
import Level1Stat from "../models/level1stat.model";
import Level2Stat from "../models/level2stat.model";
import Level3Stat from "../models/level3stat.model";
import { AuthenticatedRequest, verifyToken } from "../middleware/verifyJWT";

const router = express.Router();

router.post("/level1", async (req, res) => {
  const {
    isCorrect,
    expectedShape,
    studentListNum,
    selectedShape,
    studentGroup,
  }: IAddLevel1StatRequest = req.body;

  const student = await Student.findOne({
    where: { list_number: studentListNum, group: studentGroup },
  });

  if (!student)
    return res
      .status(404)
      .send(`Error, no student found with that list and group`);

  await Level1Stat.create({
    is_correct: isCorrect,
    expected_shape: expectedShape,
    student_id: (student as any).id,
    selected_shape: selectedShape,
  });

  return res.send("level 1 stat created");
});

router.get("/level1", verifyToken, async (req: AuthenticatedRequest, res) => {
  if (req.user?.isAdmin) {
    try {
      const stats = await Level1Stat.findAll({
        include: [{ model: Student }],
      });
      return res.send({ stats });
    } catch (error) {
      return res.status(500).send({ error });
    }
  } else {
    try {
      const stats = await Level1Stat.findAll({
        include: [{ model: Student, where: { group: req.user?.group } }],
      });
      return res.send({ stats });
    } catch (error) {
      return res.status(500).send({ error });
    }
  }
});

router.post("/level2", async (req, res) => {
  const {
    isCorrect,
    studentListNum,
    studentGroup,
    operationType,
  }: IAddLevel2StatRequest = req.body;

  const student = await Student.findOne({
    where: { list_number: studentListNum, group: studentGroup },
  });

  if (!student)
    return res
      .status(404)
      .send(`Error, no student found with that list and group`);

  await Level2Stat.create({
    is_correct: isCorrect,
    operation_type: operationType,
    student_id: (student as any).id,
  });

  return res.send("level 2 stat created");
});

router.get("/level2", verifyToken, async (req: AuthenticatedRequest, res) => {
  if (req.user?.isAdmin) {
    try {
      const stats = await Level2Stat.findAll({
        include: [{ model: Student }],
      });
      return res.send({ stats });
    } catch (error) {
      return res.status(500).send({ error });
    }
  } else {
    try {
      const stats = await Level2Stat.findAll({
        include: [{ model: Student, where: { group: req.user?.group } }],
      });
      return res.send({ stats });
    } catch (error) {
      return res.status(500).send({ error });
    }
  }
});

router.post("/level3", async (req, res) => {
  const {
    isCorrect,
    operationType,
    studentListNum,
    shapeType,
    studentGroup,
  }: IAddLevel3StatRequest = req.body;

  const student = await Student.findOne({
    where: { list_number: studentListNum, group: studentGroup },
  });

  if (!student)
    return res
      .status(404)
      .send(`Error, no student found with that list and group`);

  await Level3Stat.create({
    is_correct: isCorrect,
    operation_type: operationType,
    student_id: (student as any).id,
    shape_type: shapeType,
  });

  return res.send("level 3 stat created");
});

router.get("/level3", verifyToken, async (req: AuthenticatedRequest, res) => {
  if (req.user?.isAdmin) {
    try {
      const stats = await Level3Stat.findAll({
        include: [{ model: Student }],
      });
      return res.send({ stats });
    } catch (error) {
      return res.status(500).send({ error });
    }
  } else {
    try {
      const stats = await Level3Stat.findAll({
        include: [{ model: Student, where: { group: req.user?.group } }],
      });
      return res.send({ stats });
    } catch (error) {
      return res.status(500).send({ error });
    }
  }
});

export default router;
