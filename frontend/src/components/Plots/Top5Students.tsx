import axios from "axios";
import {
  Box,
  Select,
  Flex,
  FormControl,
  FormLabel,
  Heading,
  Tag,
  TagLeftIcon,
  TagLabel,
} from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import { ILevelScore } from "../../interfaces/levelscore.interface";
import { GrGroup } from "react-icons/gr";
import { IoMdFemale, IoMdMale } from "react-icons/io";
import { PiUserListBold } from "react-icons/pi";
import { FaRegStar } from "react-icons/fa";

export default function Top5Students() {
  const { token, user } = useAuth();

  const [stats, setStats] = useState<ILevelScore[]>([]);
  const [group, setGroup] = useState<string>("");
  const [orderBy, setorderBy] = useState<string>("top");
  const [gender, setGender] = useState<string>("A");
  const [level, setlevel] = useState<number>(1);

  useEffect(() => {
    axios
      .get(`${import.meta.env.VITE_API_URL || "http://localhost:3000"}/scores`)
      .then((res) => {
        const stats = res.data.stats;
        setStats(stats);
        if (stats.length > 0)
          setGroup((res.data.stats as ILevelScore[])[0].student.group);
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

  function filterAndSortStats() {
    // Create an object to store the highest score for each student
    const highestScores: { [studentId: string]: number } = {};

    // Iterate through each stat to find the highest score for each student
    stats.forEach((stat) => {
      const studentId = stat.student.id;
      const currentScore = stat.score;

      // Update the highest score for the student if necessary
      if (
        !highestScores[studentId] ||
        currentScore > highestScores[studentId]
      ) {
        highestScores[studentId] = currentScore;
      }
    });

    // Filter and sort the stats based on highest score per student
    return stats
      .filter((stat) => {
        const genderMatches = stat.student.gender === gender || gender === "A";
        const groupMatches = stat.student.group === group;
        const levelMatches = stat.level === level;

        return (
          genderMatches &&
          groupMatches &&
          levelMatches &&
          stat.score === highestScores[stat.student.id]
        );
      })
      .sort((a, b) =>
        orderBy === "low" ? a.score - b.score : b.score - a.score
      );
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
        <Heading mb={5}>Puntuajes de alumnos</Heading>

        <Flex gap={2}>
          <FormControl>
            <FormLabel>Categoría</FormLabel>
            <Select
              value={orderBy}
              onChange={(e) => {
                setorderBy(e.target.value);
              }}
            >
              <option value={"top"}>Top</option>
              <option value={"low"}>Low</option>
            </Select>
          </FormControl>
          <FormControl>
            <FormLabel>Nivel</FormLabel>
            <Select
              value={level}
              onChange={(e) => {
                setlevel(parseInt(e.target.value));
              }}
            >
              <option value={"1"}>1</option>
              <option value={"2"}>2</option>
              <option value={"3"}>3</option>
            </Select>
          </FormControl>
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
          <FormControl>
            <FormLabel>Grupo</FormLabel>

            <Select
              disabled={!user?.isAdmin}
              value={group}
              onChange={(e) => {
                setGroup(e.target.value);
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
        <Flex
          gap={3}
          mt={5}
          h={"95%"}
          overflowY={"auto"}
          direction={"column"}
          css={{
            "&::-webkit-scrollbar": {
              width: "4px",
            },
            "&::-webkit-scrollbar-track": {
              width: "6px",
            },
          }}
        >
          {filterAndSortStats().map((stat, i) => (
            <Stat
              score={stat.score}
              gender={stat.student.gender}
              key={i}
              listNum={stat.student.list_number}
              position={i + 1}
              group={stat.student.group}
            />
          ))}
        </Flex>
      </Box>
    </>
  );
}

interface StatProps {
  listNum: number;
  gender: string;
  position: number;
  group: string;
  score: number;
}

function Stat({ gender, listNum, position, group, score }: StatProps) {
  return (
    <>
      <Flex w={"100%"} borderWidth={1} borderRadius={"md"}>
        <Box
          px={2}
          alignContent={"center"}
          borderWidth={1}
          borderRadius={"md"}
          borderRightRadius={0}
        >
          #{position}
        </Box>
        <Tag m={2} size={"lg"} variant="subtle" colorScheme="green">
          <TagLeftIcon boxSize="12px" as={PiUserListBold} />
          <TagLabel>{listNum}</TagLabel>
        </Tag>
        <Tag m={2} size={"lg"} variant="subtle" colorScheme="cyan">
          <TagLeftIcon boxSize="12px" as={GrGroup} />
          <TagLabel>{group}</TagLabel>
        </Tag>

        <Tag
          m={2}
          size={"lg"}
          variant="subtle"
          colorScheme={gender === "F" ? "pink" : "blue"}
        >
          <TagLeftIcon
            boxSize="12px"
            as={gender === "M" ? IoMdMale : IoMdFemale}
          />
          <TagLabel>{gender === "F" ? "Mujer" : "Hombre"}</TagLabel>
        </Tag>

        <Tag m={2} size={"lg"} variant="subtle" colorScheme="cyan">
          <TagLeftIcon boxSize="12px" as={FaRegStar} />
          <TagLabel>{score}</TagLabel>
        </Tag>
      </Flex>
    </>
  );
}
