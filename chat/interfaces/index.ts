import {ChatMessage as WebUiChatMessage} from '@azure/communication-react';

export interface ChatProps {
  username: string;
  messages: WebUiChatMessage[];
  userId: string;
  onSendMessage: (message: string) => Promise<void>;
};

export interface UsernameInputProps {
  inputRef: React.RefObject<HTMLInputElement | null>;
  onSubmit: (event: React.FormEvent) => void;
};
export interface ApiMessage {
  id: string;
  type: object;
  sequenceId: string;
  content: string;
  senderDisplayName: string;
  createdOn: string;
  senderRawId: string;
  deletedOn?: string;
  editedOn?: string;
};