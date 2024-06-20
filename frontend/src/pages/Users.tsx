import {
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  Flex,
  Heading,
  Button,
  Box,
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
  Checkbox,
} from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { FaPlus } from "react-icons/fa6";
import { IoMdTrash } from "react-icons/io";
import axios from "axios";
import { useAuth } from "../hooks/useAuth";
import { IUser } from "../interfaces/user.interface";
import { FaUser, FaUserShield } from "react-icons/fa";

export default function Users() {
  const toast = useToast();

  const { user } = useAuth();
  const { isOpen, onOpen, onClose } = useDisclosure();

  const [users, setUsers] = useState<IUser[]>([]);

  useEffect(() => {
    loadUsers();
  }, []);

  function loadUsers() {
    axios
      .get(`${import.meta.env.VITE_API_URL || "http://localhost:3000"}/user`)
      .then((res) => {
        const users: IUser[] = res.data;
        setUsers(users);
      })
      .catch((err) => {
        console.log(err);
      });
  }

  function deleteUser(username: string) {
    axios
      .delete(
        `${import.meta.env.VITE_API_URL || "http://localhost:3000"}/user`,
        { data: { username } }
      )
      .then(() => {
        toast({
          title: "Usuario eliminado exitosamente",
          status: "success",
          duration: 2000,
          position: "bottom-right",
          isClosable: false,
        });
        loadUsers();
      })
      .catch((err) => {
        toast({
          title: "Error al eliminar usuario. Por favor, intente de nuevo.",
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
      <CreateModal loadUsers={loadUsers} isOpen={isOpen} onClose={onClose} />
      <Flex direction={"column"} width={"100%"} height={"96vh"}>
        <Flex mb={5} justifyContent={"space-between"}>
          <Heading>Usuarios</Heading>
          <Flex w={"50%"} gap={5} direction={"row-reverse"}>
            <Button
              onClick={onOpen}
              rightIcon={<FaPlus />}
              colorScheme="teal"
              variant="solid"
            >
              Nuevo Usuario
            </Button>
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
                <Th>Usuario</Th>
                <Th>Tipo de Usuario</Th>
                <Th>Grupo</Th>
                <Th>Acciones</Th>
              </Tr>
            </Thead>
            <Tbody>
              {users
                .filter((u) => u.username !== user?.username)
                .map((usr, i) => (
                  <Tr key={i}>
                    <Td>{usr.username}</Td>
                    <Td>
                      <Tag
                        m={2}
                        size={"lg"}
                        variant="subtle"
                        colorScheme={usr.is_admin ? "red" : "blue"}
                      >
                        <TagLeftIcon
                          boxSize="12px"
                          as={usr.is_admin ? FaUserShield : FaUser}
                        />
                        <TagLabel>
                          {usr.is_admin ? "Administrador" : "Profesor"}
                        </TagLabel>
                      </Tag>
                    </Td>

                    <Td>{usr.group === "" ? "TODOS" : usr.group}</Td>

                    <Td w={"200px"}>
                      <Button
                        leftIcon={<IoMdTrash />}
                        colorScheme="red"
                        variant="solid"
                        onClick={() => {
                          deleteUser(usr.username);
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
  loadUsers,
  onClose,
}: {
  isOpen: boolean;
  loadUsers: () => void;
  onClose: () => void;
}) {
  const toast = useToast();

  const [username, setUsername] = useState<string>("");
  const [group, setGroup] = useState<string>("");
  const [isAdmin, setAdmin] = useState<boolean>(false);
  const [password, setPassword] = useState<string>("");

  function onSubmit() {
    axios
      .post(`${import.meta.env.VITE_API_URL || "http://localhost:3000"}/user`, {
        username,
        group,
        isAdmin,
        password,
      })
      .then(() => {
        toast({
          title: "Usuario creado exitosamente",
          status: "success",
          duration: 2000,
          position: "bottom-right",
          isClosable: false,
        });

        setPassword("");
        setGroup("");
        loadUsers();

        onClose();
      })
      .catch((err) => {
        switch (err.response.status) {
          case 409:
            toast({
              title: "El usario ya existe",
              description:
                "No se pudo crear el usuario porque ya existe uno con ese nombre de usuario.",
              status: "warning",
              duration: 5000,
              position: "bottom-right",
              isClosable: true,
            });

            break;

          default:
            toast({
              title: "Error al crear usuario ",
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
          <ModalHeader>Crear nuevo usuario</ModalHeader>
          <ModalBody>
            <FormControl mb={4}>
              <Checkbox
                isChecked={isAdmin}
                onChange={() => {
                  setAdmin(!isAdmin);
                }}
              >
                Administrador
              </Checkbox>
            </FormControl>
            <FormControl>
              <FormLabel>Nombre de Usuario</FormLabel>
              <Input
                value={username}
                autoComplete="off"
                onChange={(e) => {
                  setUsername(e.target.value);
                }}
              />
            </FormControl>

            <FormControl mt={4}>
              <FormLabel>Contraseña</FormLabel>
              <Input
                value={password}
                type="text"
                autoComplete="off"
                onChange={(e) => {
                  setPassword(e.target.value);
                }}
              />
            </FormControl>

            {!isAdmin ? (
              <FormControl mt={4}>
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
          </ModalBody>

          <ModalFooter>
            <Button colorScheme="red" mr={3} onClick={onClose}>
              Cancelar
            </Button>
            <Button
              colorScheme="blue"
              isDisabled={
                (group.length === 0 && !isAdmin) ||
                password.length === 0 ||
                username.length === 0
              }
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
