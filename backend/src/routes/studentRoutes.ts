import express from "express";
import {
  ICreateStudentRequest,
  IVerifyStudentRequest,
} from "../interfaces/statRequest.interfaces";
import Student from "../models/student.model";
import { AuthenticatedRequest, verifyToken } from "../middleware/verifyJWT";

const router = express.Router();

//verify student
router.post("/verify", async (req, res) => {
  const { group, listNumber }: IVerifyStudentRequest = req.body;

  if (typeof group !== "string" || typeof listNumber !== "number")
    return res.status(400).send("Invalid request body");

  const student = await Student.findOne({
    where: { list_number: listNumber, group: group },
  });

  if (!student)
    return res.status(404).send(`No Student exists with list and group`);
  else res.send("Valid student");
});

//create student
router.post("/", verifyToken, async (req: AuthenticatedRequest, res) => {
  const { gender, group, listNumber }: ICreateStudentRequest = req.body;

  if (
    (gender !== "M" && gender !== "F") ||
    typeof group !== "string" ||
    typeof listNumber !== "number"
  )
    return res.status(400).send("Invalid request body");

  if (!req.user?.isAdmin) {
    if (req.user?.group !== group)
      return res.status(401).send("You cant create students in this group");
  }

  const student = await Student.findOne({
    where: { list_number: listNumber, group: group },
  });

  if (student) return res.status(409).send(`Student already exists`);

  try {
    await Student.create({
      gender,
      group,
      list_number: listNumber,
    });
    res.send("Student created");
  } catch (error) {
    res.status(404).send(`Error creating student: ${error}`);
  }
});

//delete student
router.delete("/", verifyToken, async (req: AuthenticatedRequest, res) => {
  const { group, listNumber }: ICreateStudentRequest = req.body;

  if (typeof group !== "string" || typeof listNumber !== "number")
    return res.status(400).send("Invalid request body");

  if (!req.user?.isAdmin) {
    if (req.user?.group !== group)
      return res
        .status(401)
        .send("You dont have access to students in this group");
  }

  const student = await Student.destroy({
    where: { list_number: listNumber, group: group },
  });

  if (student) return res.send(`Student deleted`);
  else return res.status(400).send(`No student matches group and list`);
});

//get students
router.get("/", verifyToken, async (req: AuthenticatedRequest, res) => {
  if (req.user?.isAdmin) {
    try {
      const stats = await Student.findAll();
      return res.send({ stats });
    } catch (error) {
      return res.status(500).send({ error });
    }
  } else {
    try {
      const stats = await Student.findAll({
        where: { group: req.user?.group },
      });
      return res.send({ stats });
    } catch (error) {
      return res.status(500).send({ error });
    }
  }
});

export default router;
