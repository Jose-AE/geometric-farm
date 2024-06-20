import axios from "axios";
import {
  Box,
  Select,
  Flex,
  FormControl,
  FormLabel,
  Heading,
} from "@chakra-ui/react";
import { useEffect, useState } from "react";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  Legend,
} from "recharts";
import { ILevel1Stat } from "../../interfaces/level1stat.interface";
import { useAuth } from "../../hooks/useAuth";
import { ILevel3Stat } from "../../interfaces/level3stat.interface";

const barNames = {
  Circulo: "Círculo",
  Triangulo: "Triángulo",
  Rectangulo: "Rectángulo",
  Paralelogramo: "Paralelogramo",
  Cuadrado: "Cuadrado",
  Rombo: "Rombo",
  Trapecio: "Trapecio",
  Pentagono: "Pentágono",
  Hexagono: "Hexágono",
  Octagono: "Octágono",
};

interface IData {
  name: string;
  correctPercentage: number;
  incorrectPercentage: number;
}

export default function PerformanceByAreaPlot() {
  const { token, user } = useAuth();

  const [stats, setStats] = useState<ILevel3Stat[]>([]);
  const [group, setGroup] = useState<string>("");
  const [studentListNum, setStudentListNum] = useState<number>(-1);
  const [gender, setGender] = useState<string>("A");

  useEffect(() => {
    axios
      .get(
        `${
          import.meta.env.VITE_API_URL || "http://localhost:3000"
        }/stats/level3`
      )
      .then((res) => {
        const stats = res.data.stats;
        setStats(stats);
        if (stats.length > 0)
          setGroup((res.data.stats as ILevel1Stat[])[0].student.group);
      })
      .catch((err) => {
        console.log(err);
      });
  }, [token]);

  function getGroups(): string[] {
    const groupSet = new Set<string>();
    for (const stat of stats) {
      groupSet.add(stat.student.group);
    }
    return Array.from(groupSet);
  }
  function getListNums(): number[] {
    const listNumsSet = new Set<number>();
    for (const stat of stats) {
      if (stat.student.group === group)
        listNumsSet.add(stat.student.list_number);
    }
    return Array.from(listNumsSet);
  }

  function generateData(): IData[] {
    if (stats.length <= 0) return [];

    const generatedData: IData[] = [];

    for (const shape in barNames) {
      const operationStats = stats.filter((stat) => {
        const shapeMatches = stat.shape_type === shape;

        const genderMatches =
          stat.student.gender === gender ||
          studentListNum != -1 ||
          gender === "A";

        const groupMatches = stat.student.group === group;

        const listNumMatches =
          stat.student.list_number === studentListNum || studentListNum == -1;

        return (
          shapeMatches &&
          genderMatches &&
          groupMatches &&
          listNumMatches &&
          listNumMatches &&
          stat.operation_type === "area"
        );
      });
      let correctStats = 0;

      for (let i = 0; i < operationStats.length; i++) {
        if (operationStats[i].is_correct) correctStats++;
      }

      let correctPercentage = (correctStats / operationStats.length) * 100;
      let incorrectPercentage = 100 - correctPercentage;

      if (operationStats.length <= 0) {
        correctPercentage = 0;
        incorrectPercentage = 0;
      } else {
        correctPercentage = Math.round(correctPercentage * 10) / 10;
        incorrectPercentage = Math.round(incorrectPercentage * 10) / 10;
      }

      generatedData.push({
        name: barNames[shape as keyof typeof barNames],
        correctPercentage,
        incorrectPercentage,
      });
    }
    return generatedData;
  }

  return (
    <>
      <Box
        p={5}
        paddingBottom={40}
        borderWidth={2}
        borderRadius={"lg"}
        w={"100%"}
        h={"100%"}
      >
        <Heading mb={5}>Rendimiento por área de figura</Heading>

        <Flex gap={2}>
          <FormControl>
            <FormLabel>Número de lista de estudiante</FormLabel>
            <Select
              value={studentListNum}
              onChange={(e) => {
                setStudentListNum(parseInt(e.target.value));
              }}
            >
              <option value={-1}>Todos</option>
              {getListNums().map((num, i) => (
                <option key={i} value={num}>
                  {num}
                </option>
              ))}
            </Select>
          </FormControl>
          {studentListNum === -1 ? (
            <FormControl>
              <FormLabel>Género</FormLabel>
              <Select
                value={gender}
                onChange={(e) => {
                  setGender(e.target.value);
                }}
              >
                <option value={"A"}>Ambos</option>
                <option value={"M"}>Hombre</option>
                <option value={"F"}>Mujer</option>
              </Select>
            </FormControl>
          ) : null}
          <FormControl>
            <FormLabel>Grupo</FormLabel>

            <Select
              disabled={!user?.isAdmin}
              value={group}
              onChange={(e) => {
                setGroup(e.target.value);
                setStudentListNum(-1);
              }}
            >
              {getGroups().map((group, i) => (
                <option key={i} value={group}>
                  {group}
                </option>
              ))}
            </Select>
          </FormControl>
        </Flex>
        <ResponsiveContainer width="100%" height="100%">
          <BarChart
            width={500}
            height={300}
            data={generateData()}
            margin={{
              top: 20,
              right: 30,
              left: 20,
              bottom: 5,
            }}
          >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip />
            <Legend />
            <Bar
              name={"%Correcto"}
              dataKey="correctPercentage"
              stackId="a"
              fill="#42bd46"
            />
            <Bar
              name={"%Incorrecto"}
              dataKey="incorrectPercentage"
              stackId="a"
              fill="#d44333"
            />
          </BarChart>
        </ResponsiveContainer>
      </Box>
    </>
  );
}
