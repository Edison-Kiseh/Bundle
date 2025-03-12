'use client'
import React, { useState } from "react";
import Button from "@mui/material/Button";
import Modal from "./Modal";

const LeaveChatButton: React.FC = () => {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <>
            <Button
                type="submit"
                variant="contained"
                color="error"
                onClick={() => setIsOpen(true)}
                sx={{ position: 'fixed', top: 16, right: 16, zIndex: 1 }}
            >
                Leave Chat
            </Button>
            {isOpen && <Modal onClose={() => setIsOpen(false)} />}
        </>
    );
};

export default LeaveChatButton;