import React from 'react';
import ChatComponent from '../../components/Chat';
import LeaveChatButton from '../../components/LeaveChatButton';

export default function ChatPage() {
  return (
    <div className="containerWelcome">
        <LeaveChatButton />
        <ChatComponent />
    </div>
  );
}