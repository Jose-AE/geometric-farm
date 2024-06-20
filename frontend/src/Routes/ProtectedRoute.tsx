import React from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

interface ProtectedRouteProps {
  children: React.ReactNode;
  requireAdmin?: boolean;
}

export default function ProtectedRoute({
  children,
  requireAdmin = false,
}: ProtectedRouteProps) {
  const { isLoggedIn, user } = useAuth();

  if (!requireAdmin)
    return (
      <>{isLoggedIn() ? <>{children}</> : <Navigate to="/login" replace />}</>
    );
  else
    return (
      <>
        {isLoggedIn() && user?.isAdmin ? (
          <>{children}</>
        ) : (
          <Navigate to="/" replace />
        )}
      </>
    );
}
