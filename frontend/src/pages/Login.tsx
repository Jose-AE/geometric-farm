import {
  Flex,
  Box,
  Input,
  Stack,
  Button,
  useColorModeValue,
  InputGroup,
  InputLeftElement,
  useToast,
  Image,
} from "@chakra-ui/react";
import { MdOutlineLock, MdOutlinePerson } from "react-icons/md";

import Logo from "../assets/Logo.png";

import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export default function Login() {
  const navigate = useNavigate();
  const toast = useToast();

  const { login } = useAuth();

  const [loading, setLoading] = useState(false);
  const [password, setPassword] = useState("");
  const [username, setUsername] = useState("");

  async function HandleLogin() {
    setLoading(true);

    const succ = await login(username, password);

    if (succ) {
      navigate("/");
    } else {
      toast({
        position: "bottom-right",
        title: "Error de Autenticación",
        description:
          "El usuario o la contraseña es incorrecta. Por favor, inténtelo de nuevo.",
        status: "error",
        duration: 3000,
        isClosable: true,
      });

      setLoading(false);
    }
  }

  return (
    <>
      <Flex
        h={"100vh"}
        align={"center"}
        justify={"center"}
        bg={useColorModeValue("gray.50", "gray.800")}
      >
        <Stack spacing={8} mx={"auto"} maxW={"lg"}>
          <Stack align={"center"}>
            <Image bg={""} src={Logo} />
          </Stack>
          <Box
            rounded={"lg"}
            bg={useColorModeValue("white", "gray.700")}
            boxShadow={"lg"}
            p={8}
          >
            <Stack spacing={4}>
              <InputGroup>
                <InputLeftElement pointerEvents="none">
                  <MdOutlinePerson />
                </InputLeftElement>
                <Input
                  autoComplete="off"
                  onChange={(e) => {
                    setUsername(e.target.value);
                  }}
                  placeholder="Usuario"
                  type="text"
                />
              </InputGroup>

              <InputGroup>
                <InputLeftElement pointerEvents="none">
                  <MdOutlineLock />
                </InputLeftElement>
                <Input
                  autoComplete="off"
                  onChange={(e) => {
                    setPassword(e.target.value);
                  }}
                  placeholder="Contraseña"
                  type="password"
                />
              </InputGroup>

              <Button
                isDisabled={username.length === 0 || password.length === 0}
                w="100%"
                isLoading={loading}
                colorScheme="blue"
                fontSize={{ base: "xs", md: "md" }}
                onClick={() => {
                  HandleLogin();
                }}
              >
                Iniciar sesión
              </Button>
            </Stack>
          </Box>
        </Stack>
      </Flex>
    </>
  );
}
