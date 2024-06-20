import React from "react";
import ReactDOM from "react-dom/client";
import { ChakraProvider } from "@chakra-ui/react";
import { RouterProvider } from "react-router-dom";
import AuthProvider from "./Providers/AuthProvider.tsx";
import router from "./Routes/Router.tsx";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <ChakraProvider>
      <AuthProvider>
        <RouterProvider router={router} />
      </AuthProvider>
    </ChakraProvider>
  </React.StrictMode>
);

///Hide warnings
const originalWarn = console.error;

console.error = (message, ...args) => {
  if (
    typeof message === "string" &&
    message.includes("Support for defaultProp")
  ) {
    return;
  }
  originalWarn(message, ...args);
};
