import { useEffect, useRef, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import { toast } from 'react-toastify';
import { ChatMessage as WebUiChatMessage, SystemMessage, MessageStatus, MessageContentType } from '@azure/communication-react';
import { v4 as uuidv4 } from 'uuid';

/**
 * Custom hook to manage a SignalR connection for real-time chat functionality.
 * 
 * @param username - The username of the current user.
 * @param userId - The unique identifier of the current user.
 * @param setMessages - Function to update the state of messages in the chat.
 * @returns The SignalR connection instance.
 */
export const useSignalRConnection = (username: string | null, userId: string, setMessages: React.Dispatch<React.SetStateAction<(WebUiChatMessage | SystemMessage)[]>>) => {
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const initializedRef = useRef(false);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  
  useEffect(() => {
    // Prevent re-initialization of the connection
    if (initializedRef.current) {
      return;
    }
    initializedRef.current = true;

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env.NEXT_PUBLIC_API_BASE_URL}/chatHub`, {
        withCredentials: true
      })
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: retryContext => {
          // Calculate the delay before the next reconnection attempt
          // This ensures exponential backoff with a maximum delay
          return Math.min(retryContext.previousRetryCount * 1000, 10000);
        }
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connectionRef.current = connection;
    setConnection(connection);

    /**
     * Function to establish the SignalR connection.
     * This function attempts to start the connection and handles any errors that occur during the process.
     * If the connection fails, it retries after a delay, ensuring that the chat functionality remains available.
     */
    const startConnection = async () => {
      try {
        await connection.start();
        toast.success('Connected to chat successfully.');
      } catch {
        toast.error('Failed to connect to chat, retrying...');
        setTimeout(startConnection, 5000); 
      }
    };
    // Start the connection with a slight delay to ensure all configurations are set
    setTimeout(startConnection, 100); 

    /**
     * Event handler for receiving messages.
     * This function is called whenever a new message is received from the server.
     * It updates the state with the new message, ensuring that the chat UI is updated in real-time.
     */
    connection.on('ReceiveMessage', (user: string, message: string, isNotification: boolean) => handleReceiveMessage(user, message, isNotification, userId, username, setMessages));

    /**
     * Event handler for connection closure.
     * This function is called whenever the connection is closed.
     * It attempts to reconnect after a delay, ensuring that the chat functionality remains available.
     */
    connection.onclose(async () => {
      toast.success('Disconnected from chat successfully.');
      await new Promise(resolve => setTimeout(resolve, 5000));
      try {
        await connection.start();
        toast.success('Reconnected to chat successfully.');
      } catch {
        toast.error('Failed to reconnect to chat.');
      }
    });

    // Cleanup function to stop the connection when the component unmounts
    return () => {
      /**
       * Function to stop the SignalR connection.
       * This function ensures that the connection is properly closed when the component is unmounted,
       * preventing potential memory leaks and ensuring a clean disconnection.
       */
      const stopConnection = async () => {
        if(connectionRef.current){
          try {
            await connection.stop();
            toast.success('Disconnected from chat successfully.');
          } catch {
            toast.error('Failed to disconnect from chat.');
          }
        }
      };
      stopConnection();
    };
  }, [username, userId, setMessages]);

  return connection;
};

/**
 * Handles the reception of a new chat message and updates the state.
 * 
 * @param user - The username of the sender.
 * @param message - The content of the message.
 * @param isNotification - Whether the message is a notification.
 * @param userId - The unique identifier of the current user.
 * @param username - The username of the current user.
 * @param setMessages - Function to update the state of messages in the chat.
 */
const handleReceiveMessage = (user: string, message: string, isNotification: boolean, userId: string, username: string | null, setMessages: React.Dispatch<React.SetStateAction<(WebUiChatMessage | SystemMessage)[]>>) => {
  const newMessage = isNotification
    ? {
        messageType: 'system' as const,
        systemMessageType: 'content' as const,
        iconName: 'PeopleAdd',
        content: message,
        createdOn: new Date(),
        messageId: uuidv4(),
      }
    : {
        messageType: 'chat' as const,
        contentType: 'text' as MessageContentType,
        senderId: userId,
        senderDisplayName: user,
        messageId: uuidv4(),
        content: message,
        createdOn: new Date(),
        mine: user === username,
        attached: false,
        status: 'seen' as MessageStatus
      };
  setMessages(prevMessages => {
    if (!prevMessages.some(msg => msg.messageId === newMessage.messageId)) {
      return [...prevMessages, newMessage];
    }
    return prevMessages;
  });
};