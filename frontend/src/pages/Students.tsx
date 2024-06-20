import {
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  Text,
  Flex,
  Heading,
  Button,
  Box,
  Select,
  TagLeftIcon,
  Tag,
  TagLabel,
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalBody,
  FormControl,
  FormLabel,
  Input,
  ModalFooter,
  useDisclosure,
  useToast,
} from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { FaPlus } from "react-icons/fa6";
import { IoMdFemale, IoMdMale, IoMdTrash } from "react-icons/io";
import { IStudent } from "../interfaces/sudent.interface";
import axios from "axios";
import { useAuth } from "../hooks/useAuth";

export default function Students() {
  const toast = useToast();

  const { user } = useAuth();
  const { isOpen, onOpen, onClose } = useDisclosure();

  const [students, setStudents] = useState<IStudent[]>([]);
  const [selectedGroup, setSelectedGroup] = useState<string>("");
  const [allGroups, setAllGroups] = useState<string[]>([]);

  useEffect(() => {
    loadStudents();
  }, []);

  function loadStudents(refreshSelectedGroup = true) {
    axios
      .get(`${import.meta.env.VITE_API_URL || "http://localhost:3000"}/student`)
      .then((res) => {
        const students: IStudent[] = res.data.stats;
        setStudents(students);

        const groupSet = new Set<string>();
        students.map((s) => {
          groupSet.add(s.group);
        });
        setAllGroups(Array.from(groupSet));
        if (refreshSelectedGroup) setSelectedGroup(Array.from(groupSet)[0]);
      })
      .catch((err) => {
        console.log(err);
      });
  }

  function deleteStudent(g: string, num: number) {
    axios
      .delete(
        `${import.meta.env.VITE_API_URL || "http://localhost:3000"}/student`,
        { data: { group: g, listNumber: num } }
      )
      .then(() => {
        toast({
          title: "Estudiante eliminado exitosamente",
          status: "success",
          duration: 2000,
          position: "bottom-right",
          isClosable: false,
        });
        loadStudents(false);
      })
      .catch((err) => {
        toast({
          title: "Error al eliminar estudiante. Por favor, intente de nuevo.",
          status: "error",
          duration: 2000,
          position: "bottom-right",
          isClosable: false,
        });
        console.log(err);
      });
  }

  return (
    <>
      <CreateModal
        loadStudents={loadStudents}
        isOpen={isOpen}
        onClose={onClose}
      />
      <Flex direction={"column"} width={"100%"} height={"96vh"}>
        <Flex mb={5} justifyContent={"space-between"}>
          <Heading>Estudiantes</Heading>
          <Flex w={"50%"} gap={5} direction={"row-reverse"}>
            <Button
              onClick={onOpen}
              rightIcon={<FaPlus />}
              colorScheme="teal"
              variant="solid"
            >
              Nuevo Estudiante
            </Button>
            {user?.isAdmin ? (
              <Flex alignItems={"center"}>
                <Text as={"b"} mr={2}>
                  Grupo:{" "}
                </Text>
                <Select
                  w={100}
                  value={selectedGroup}
                  onChange={(e) => {
                    setSelectedGroup(e.target.value);
                  }}
                >
                  {allGroups.map((g, i) => (
                    <option key={i} value={g}>
                      {g}
                    </option>
                  ))}
                </Select>
              </Flex>
            ) : null}
          </Flex>
        </Flex>
        <Box
          overflowY="scroll"
          maxH="100%"
          css={{
            "&::-webkit-scrollbar": {
              width: "4px",
            },
            "&::-webkit-scrollbar-track": {
              width: "6px",
            },
          }}
        >
          <Table variant="striped" colorScheme="gray.100">
            <Thead
              zIndex={1}
              borderRadius={"5px"}
              position="sticky"
              top={0}
              bgColor="lightblue"
            >
              <Tr>
                <Th># Lista</Th>
                <Th>Grupo</Th>
                <Th>Género</Th>
                <Th>Acciones</Th>
              </Tr>
            </Thead>
            <Tbody>
              {students
                .filter((s) => s.group === selectedGroup)
                .map((student, i) => (
                  <Tr key={i}>
                    <Td>{student.list_number}</Td>
                    <Td>{student.group}</Td>
                    <Td>
                      <Tag
                        m={2}
                        size={"lg"}
                        variant="subtle"
                        colorScheme={student.gender === "F" ? "pink" : "blue"}
                      >
                        <TagLeftIcon
                          boxSize="12px"
                          as={student.gender === "M" ? IoMdMale : IoMdFemale}
                        />
                        <TagLabel>
                          {student.gender === "F" ? "Mujer" : "Hombre"}
                        </TagLabel>
                      </Tag>
                    </Td>

                    <Td w={"200px"}>
                      <Button
                        leftIcon={<IoMdTrash />}
                        colorScheme="red"
                        variant="solid"
                        onClick={() => {
                          deleteStudent(student.group, student.list_number);
                        }}
                      >
                        Eliminar
                      </Button>
                    </Td>
                  </Tr>
                ))}
            </Tbody>
          </Table>
        </Box>
      </Flex>
    </>
  );
}

function CreateModal({
  isOpen,
  loadStudents,
  onClose,
}: {
  isOpen: boolean;
  loadStudents: (g: boolean) => void;
  onClose: () => void;
}) {
  const { user } = useAuth();

  const toast = useToast();

  const [group, setGroup] = useState<string>("");
  const [gender, setGender] = useState<string>("M");
  const [listNum, setListNum] = useState<string>("");

  function onSubmit() {
    axios
      .post(
        `${import.meta.env.VITE_API_URL || "http://localhost:3000"}/student`,
        {
          group: user?.isAdmin ? group : user?.group,
          gender,
          listNumber: parseInt(listNum),
        }
      )
      .then(() => {
        toast({
          title: "Estudiante creado exitosamente",
          status: "success",
          duration: 2000,
          position: "bottom-right",
          isClosable: false,
        });

        setListNum("");
        setGroup("");
        loadStudents(false);

        onClose();
      })
      .catch((err) => {
        switch (err.response.status) {
          case 409:
            toast({
              title: "El estudiante ya existe",
              description:
                "No se pudo crear el estudiante porque ya existe uno con ese grupo y número de lista.",
              status: "warning",
              duration: 5000,
              position: "bottom-right",
              isClosable: true,
            });

            break;

          default:
            toast({
              title: "Error al crear estudiante ",
              description: err.message,
              status: "error",
              duration: 3000,
              position: "bottom-right",
              isClosable: true,
            });
            break;
        }
      });
  }

  return (
    <>
      <Modal isOpen={isOpen} onClose={onClose}>
        <ModalOverlay />
        <ModalContent>
          <ModalHeader>Crear nuevo estudiante</ModalHeader>
          <ModalBody>
            {user?.isAdmin ? (
              <FormControl>
                <FormLabel>Grupo</FormLabel>
                <Input
                  value={group}
                  autoComplete="off"
                  onChange={(e) => {
                    setGroup(e.target.value);
                  }}
                />
              </FormControl>
            ) : null}

            <FormControl mt={4}>
              <FormLabel># Lista</FormLabel>
              <Input
                value={listNum}
                type="number"
                autoComplete="off"
                onChange={(e) => {
                  setListNum(e.target.value);
                }}
              />
            </FormControl>

            <FormControl mt={4}>
              <FormLabel>Género</FormLabel>
              <Select
                value={gender}
                onChange={(e) => {
                  setGender(e.target.value);
                }}
              >
                <option value={"M"}>Hombre</option>
                <option value={"F"}>Mujer</option>
              </Select>
            </FormControl>
          </ModalBody>

          <ModalFooter>
            <Button colorScheme="red" mr={3} onClick={onClose}>
              Cancelar
            </Button>
            <Button
              colorScheme="blue"
              isDisabled={group?.length === 0 || listNum.length === 0}
              onClick={onSubmit}
            >
              Crear
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
    </>
  );
}
