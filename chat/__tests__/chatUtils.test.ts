import axiosInstance from '../utils/Axios/axiosConfig';
import { createChat, addUserToChat, sendMessage, mapSystemMessage, mapChatMessage } from '../utils/chatUtils';
import * as signalR from '@microsoft/signalr';
import { ApiMessage } from '../interfaces';
import { SystemMessage, ChatMessage as WebUiChatMessage } from '@azure/communication-react';

jest.mock('../utils/Axios/axiosConfig');

describe('chatUtils', () => {
  const mockedAxios = axiosInstance as jest.Mocked<typeof axiosInstance>;
  const mockConnection = {
    invoke: jest.fn(),
  } as unknown as signalR.HubConnection;

  afterEach(() => {
    jest.clearAllMocks();
  });

  describe('createChat', () => {
    it('should creates a new chat when requesting a new chat to the chat service', async () => {
      mockedAxios.post.mockResolvedValueOnce({});

      await createChat();

      expect(mockedAxios.post).toHaveBeenCalledWith('/chat/', {
        topic: 'Public Chat',
      });
    });

    it('should handle error when chat service is not available', async () => {
      mockedAxios.post.mockRejectedValueOnce(new Error('Service Unavailable'));

      await expect(createChat()).rejects.toThrow('Service Unavailable');
    });

    it('should handle conflict error when creating a chat twice', async () => {
      mockedAxios.post.mockResolvedValueOnce({});
      await createChat();

      mockedAxios.post.mockRejectedValueOnce(new Error('Conflict. Chat already exists.'));
      await expect(createChat()).rejects.toThrow('Conflict. Chat already exists.');
    });
  });

  describe('addUserToChat', () => {
    it('should add a user to the chat and return userId', async () => {
      const username = 'User 123';
      const mockResponse = { data: { userId: 'user123' } };
      mockedAxios.post.mockResolvedValueOnce(mockResponse);

      const result = await addUserToChat(username);

      expect(mockedAxios.post).toHaveBeenCalledWith('/chat/user/', {
        displayName: username,
      });
      expect(result).toEqual({ userId: 'user123' });
    });

    it('should handle conflict error when adding a user to the chat', async () => {
      const username = 'User 123';
      mockedAxios.post.mockRejectedValueOnce(new Error('Conflict. Value already used.'));

      await expect(addUserToChat(username)).rejects.toThrow('Conflict. Value already used.');
    });
  });

  describe('sendMessage', () => {
    it('should send a message and invoke the connection', async () => {
      const message = 'Hello World';
      const username = 'User 123';
      mockedAxios.post.mockResolvedValueOnce({});
      (mockConnection.invoke as jest.Mock).mockResolvedValueOnce(Promise.resolve());

      await sendMessage(message, username, mockConnection);

      expect(mockConnection.invoke).toHaveBeenCalledWith('SendMessage', username, message);
      expect(mockedAxios.post).toHaveBeenCalledWith('/chat/message/', {
        message: message,
        displayName: username,
      });
    });

    it('should not send a message if connection is null', async () => {
      const message = 'Hello World';
      const username = 'User 123';

      await sendMessage(message, username, null);

      expect(mockConnection.invoke).not.toHaveBeenCalled();
      expect(mockedAxios.post).not.toHaveBeenCalled();
    });
  });

  describe('mapSystemMessage', () => {
    it('should map an ApiMessage to a SystemMessage', () => {
      const apiMessage: ApiMessage = {
        id: '1',
        content: 'System message',
        createdOn: '2025-03-10T10:00:00Z',
        senderDisplayName: 'System',
        senderRawId: 'system',
        type: {},
        sequenceId: '1',
      };
      const expectedSystemMessage: SystemMessage = {
        messageType: 'system',
        systemMessageType: 'content',
        iconName: 'PeopleAdd',
        content: apiMessage.content,
        createdOn: new Date(apiMessage.createdOn),
        messageId: apiMessage.id,
      };

      const result = mapSystemMessage(apiMessage);
      expect(result).toEqual(expectedSystemMessage);
    });
  });

  describe('mapChatMessage', () => {
    it('should map an ApiMessage to a WebUiChatMessage', () => {
      const apiMessage: ApiMessage = {
        id: '1',
        content: 'Chat message',
        createdOn: '2025-03-10T10:00:00Z',
        senderDisplayName: 'User',
        senderRawId: 'user123',
        type: {},
        sequenceId: '1',
      };

      const userId = 'user123';

      const expectedChatMessage: WebUiChatMessage = {
        messageType: 'chat',
        contentType: 'text',
        senderId: apiMessage.senderRawId || 'unknown',
        senderDisplayName: apiMessage.senderDisplayName || 'unknown',
        messageId: apiMessage.id,
        content: apiMessage.content,
        createdOn: new Date(apiMessage.createdOn),
        mine: apiMessage.senderRawId === userId,
        attached: false,
        status: 'seen',
        deletedOn: undefined,
        editedOn: undefined,
      };

      const result = mapChatMessage(apiMessage, userId);
      expect(result).toEqual(expectedChatMessage);
    });
  });
});