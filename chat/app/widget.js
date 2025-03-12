import React from "react";
import { createRoot } from "react-dom/client";
import ChatComponent from '../components/Chat';

window.ChatWidget = {
  init: (selector) => {
    const container = document.querySelector(selector);
    if (!container) {
      console.error("ChatWidget: Invalid selector provided");
      return;
    }
    const root = createRoot(container);
    root.render(<ChatComponent />);
  },
};
