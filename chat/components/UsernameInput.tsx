"use client";
import React, { useRef, useEffect, useState } from "react";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Box from "@mui/material/Box";
import { useRouter } from 'next/navigation';
import { createChat, addUserToChat } from '../utils/chatUtils';
import { styles } from "../styles/UsernameInputStyles";
import {toast} from 'react-toastify';

const UsernameInput: React.FC = () => {
  const usernameRef = useRef<HTMLInputElement>(null);
  const router = useRouter();
  const [isClient, setIsClient] = useState(false);

  useEffect(() => {
    setIsClient(true);
    localStorage.removeItem('username');
    localStorage.removeItem('userId');
  }, []);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    const username = usernameRef.current?.value;
    if (username) {
      try {
        await createChat();
        const responseAddUser = await addUserToChat(username);
        if (responseAddUser) {
          localStorage.setItem('userId', responseAddUser.userId);
          localStorage.setItem('username', username);
          router.push('/chat');
        }
      } catch (error) {
        if (error instanceof Error) {
          toast.error(error.message);
        } else {
          toast.error('An unknown error occurred. Please try again later.');
        }
      }
    }
  };

  if (!isClient) {
    return null;
  }

  return (
    <Box component="form" onSubmit={handleSubmit} sx={styles.boxStyles}>
      <TextField label="Username" variant="outlined" inputRef={usernameRef} required />
      <Button type="submit" variant="contained" color="primary">
        Submit
      </Button>
    </Box>
  );
};

export default UsernameInput;