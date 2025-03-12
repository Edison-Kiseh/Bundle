"use client";
import React, { useEffect, useState, useRef, useCallback } from 'react';
import { MessageThread, SendBox, ChatMessage as WebUiChatMessage, SystemMessage } from '@azure/communication-react';
import { useRouter } from 'next/navigation';
import { fetchMessages } from '../utils/chatUtils';
import { toast } from 'react-toastify';
import { useSignalRConnection } from '../utils/Hooks/useSignalRConnection';
import { useHandleSendMessage } from '../utils/Hooks/useHandleSendMessage';
import ReactDOM from 'react-dom';

declare global {
  interface Window {
    renderChatWidget: (containerId: string) => void;
  }
}

const ChatComponent = () => {
  const [username, setUsername] = useState<string | null>(null);
  const [messages, setMessages] = useState<(WebUiChatMessage | SystemMessage)[]>([]);
  const [userId, setUserId] = useState<string>('');
  const router = useRouter();
  const messageThreadRef = useRef<HTMLDivElement>(null);

  /**
   * Function to scroll to the bottom of the message thread.
   * This ensures that the latest messages are always visible.
   */
  const scrollToBottom = () => {
    if (messageThreadRef.current) {
      messageThreadRef.current.scrollTop = messageThreadRef.current.scrollHeight;
    }
  };

  useEffect(() => {
    const storedUserId = localStorage.getItem('userId') ?? '';
    setUserId(storedUserId);
  }, []);

  const connection = useSignalRConnection(username, userId, setMessages);
  const handleSendMessage = useHandleSendMessage(connection, username, userId, setMessages);

  /**
   * Function to initialize the chat by fetching initial messages and setting user information.
   * This function is memoized using useCallback to avoid unnecessary re-renders.
   */
  const initializeChat = useCallback(async () => {
    const storedUserId = localStorage.getItem('userId');
    const storedUsername = localStorage.getItem('username');
    if (!storedUserId) {
      throw new Error('User ID not found');
    }
    if (!storedUsername) {
      throw new Error('Username not found');
    }
    try {
      setUserId(storedUserId);
      setUsername(storedUsername);
      const initialMessages = await fetchMessages(storedUserId);
      setMessages(initialMessages);
    } catch {
      throw new Error('Failed to fetch initial messages');
    }
  }, []);

  useEffect(() => {
    const fetchData = async () => {
      try {
        await initializeChat();
      } catch (error) {
        if (error instanceof Error) {
          toast.error(error.message);
        } else {
          toast.error('An unknown error occurred. Please try again later.');
        }
        router.push('/');
      }
    };
    fetchData();
  }, [router, initializeChat]);

  // Effect to scroll to the bottom of the message thread whenever messages change
  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  // If the username is not set, do not render the chat component
  if (username === null) {
    return null;
  }

  return (
    <div className="chat-container">
      <div className="message-thread" ref={messageThreadRef}>
        <MessageThread userId={userId} messages={messages} showMessageStatus={true} />
      </div>
      <div className="send-box">
        <SendBox
          disabled={false}
          onSendMessage={async (message) => {
            await handleSendMessage(message);
          }}
          onTyping={async () => {
            return;
          }}
        />
      </div>
    </div>
  );
};

// Function to mount the widget globally
window.renderChatWidget = (containerId) => {
  const container = document.getElementById(containerId);
  if (container) {
  ReactDOM.render(<ChatComponent />, container);
  }
};

export default ChatComponent;