import axiosInstance from './Axios/axiosConfig';
import { ChatMessage as WebUiChatMessage, MessageStatus,SystemMessage, MessageContentType} from '@azure/communication-react';
import { ApiMessage } from '../interfaces';


export const mapSystemMessage = (msg: ApiMessage): SystemMessage => {
  return {
    messageType: 'system' as const,
    systemMessageType: 'content' as const,
    iconName: 'PeopleAdd',
    content: msg.content,
    createdOn: new Date(msg.createdOn),
    messageId: msg.id,
  };
};

export const mapChatMessage = (msg: ApiMessage, userId: string): WebUiChatMessage => {
  return {
    messageType: 'chat' as const,
    contentType: 'text' as MessageContentType,
    senderId: msg.senderRawId || 'unknown',
    senderDisplayName: msg.senderDisplayName || 'unknown',
    messageId: msg.id,
    content: msg.content,
    createdOn: new Date(msg.createdOn),
    mine: msg.senderRawId === userId,
    attached: false,
    status: 'seen' as MessageStatus,
    deletedOn: msg.deletedOn ? new Date(msg.deletedOn) : undefined,
    editedOn: msg.editedOn ? new Date(msg.editedOn) : undefined,
  };
};

export const fetchMessages = async (userId: string): Promise<(WebUiChatMessage | SystemMessage)[]> => {
  if (!userId) {
    throw new Error('User ID not found');
  }
  const messagesResponse = await axiosInstance.get(`/chat/messages/`);
  const messages: ApiMessage[] = messagesResponse.data;

  const systemMessages = messages
    .filter(msg => msg.senderDisplayName === 'System')
    .map(mapSystemMessage);

  const chatMessages = messages
    .filter(msg => msg.senderDisplayName !== 'System')
    .map(msg => mapChatMessage(msg, userId));

  return [...systemMessages, ...chatMessages].reverse();
};

export const sendMessage = async (message: string,username: string, connection: signalR.HubConnection | null
): Promise<void> => {
  if (message && connection) {
    await connection.invoke('SendMessage',username, message);
    await axiosInstance.post(`/chat/message/`, {
      message: message,
      displayName: username
    });
  }
};

export const createChat = async (): Promise<void> => {
  await axiosInstance.post(`/chat/`, {
    topic: "Public Chat",
  });
};

export const addUserToChat = async (username: string): Promise<{ userId: string } | undefined> => {
  const response = await axiosInstance.post(`/chat/user/`, {
    displayName: username
  });
  return response.data;
};

export const removeUserFromChat = async (username: string): Promise<void> => {
  await axiosInstance.delete(`/chat/user/${username}`);
};