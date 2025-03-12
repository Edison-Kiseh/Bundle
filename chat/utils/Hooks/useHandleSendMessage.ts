import { ChatMessage as WebUiChatMessage, MessageStatus, SystemMessage } from '@azure/communication-react';
import { v4 as uuidv4 } from 'uuid';
import { toast } from 'react-toastify';
import { sendMessage, fetchMessages } from '../../utils/chatUtils';

/**
 * Custom hook to handle sending messages in a chat application.
 * 
 * @param connection - The SignalR connection instance.
 * @param username - The username of the current user.
 * @param userId - The unique identifier of the current user.
 * @param setMessages - Function to update the state of messages in the chat.
 * @returns A function to handle sending messages.
 */
export const useHandleSendMessage = (connection: signalR.HubConnection | null, username: string | null, userId: string, setMessages: React.Dispatch<React.SetStateAction<(WebUiChatMessage | SystemMessage)[]>>) => {
  const handleSendMessage = async (message: string) => {
    const tempMessageId = uuidv4();
    const newMessage: WebUiChatMessage = {
      messageType: 'chat',
      contentType: 'text',
      senderId: userId,
      senderDisplayName: username ?? 'Unknown User',
      messageId: tempMessageId, 
      content: message,
      createdOn: new Date(),
      mine: true, 
      attached: false,
      status: 'sending' as MessageStatus
    };

    setMessages(prevMessages => [...prevMessages, newMessage]);

    try {
      if (username) {
        await sendMessage(message, username, connection);

        const updatedMessages = await fetchMessages(userId);
        // Update the message status to "sent" if the message ID matches the temporary ID
        if (Array.isArray(updatedMessages)) {
          setMessages(prevMessages =>
            updatedMessages.map(msg =>
              msg.messageId === tempMessageId ? { ...msg, status: 'sent' as MessageStatus } : msg
            )
          );
        }
      } else {
        toast.error('Username is null');
      }
    } catch {
      toast.error('Failed to send message, please try later.');
      setMessages(prevMessages =>
        prevMessages.map(msg =>
          msg.messageId === tempMessageId ? { ...msg, status: 'failed' as MessageStatus } : msg
        )
      );
    }
  };

  return handleSendMessage;
};