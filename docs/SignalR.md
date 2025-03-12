# Using SignalR for Real-Time Chat with Azure Communication Services (ACS)

## Introduction

This documentation explains why and how SignalR is used to implement real-time chat functionality in our application, in conjunction with Azure Communication Services (ACS). SignalR is an ASP.NET library that simplifies adding real-time web functionality to your applications. It enables bidirectional communication between the server and the client, which is essential for a responsive and interactive chat application.

## Why Use SignalR?

### Real-Time Bidirectional Communication

SignalR enables real-time bidirectional communication between the server and the client. This means that messages can be sent and received instantly, without needing to refresh the page or make repeated requests to the server. This functionality is crucial for a chat application, where users expect to see new messages appear immediately.

### Connection Management

SignalR automatically manages connections, disconnections, and reconnections. This ensures that users remain connected to the chat even in the event of a temporary connection loss. SignalR uses different transports (WebSockets, Server-Sent Events, Long Polling) to adapt to the capabilities of the client and server, ensuring a reliable connection.

### Integration with Azure Communication Services (ACS)

Azure Communication Services (ACS) provides APIs to add communication capabilities (chat, voice, video) to your applications. By using SignalR with ACS, we can leverage ACS's real-time communication capabilities while using SignalR to manage connections and message transmission.

## How to Use SignalR with ACS

### Server-Side

1. **Creating the SignalR Hub**:
   - The SignalR hub is responsible for managing connections and broadcasting messages to connected clients. It is integrated with ACS to send and receive messages.

2. **Configuring SignalR**:
   - SignalR is configured in the `Program.cs` file to add the necessary services and map the hub.

### Client-Side

1. **Initializing the SignalR Connection**:
   - A custom hook `useSignalRConnection` is used to manage the SignalR connection. This hook initializes the connection, handles message reception and connection closure events, and cleans up the connection when the component is unmounted.

2. **Handling Message Sending**:
   - Another custom hook `useHandleSendMessage` is used to handle message sending. This hook creates a temporary message, attempts to send it via SignalR, and updates the message status based on the result.

### How the Hooks Work

#### `useSignalRConnection`

- **Initializing the Connection**:
  - The SignalR connection is initialized with the hub URL and configured to automatically reconnect in case of disconnection. This ensures that users remain connected to the chat even in the event of a temporary connection loss.

- **Handling Received Messages**:
  - An event handler is configured to listen for messages received from the server. When a message is received, it is added to the message state, updating the user interface in real-time.

- **Handling Disconnections**:
  - Another event handler is configured to handle disconnections. In case of disconnection, the connection is re-established after a delay, ensuring a smooth user experience.

#### `useHandleSendMessage`

- **Creating Temporary Messages**:
  - When a user sends a message, a temporary message is created with a "sending" status. This allows the message to be displayed immediately in the user interface.

- **Sending Messages**:
  - The message is sent to the server via SignalR. If the send is successful, the message status is updated to "sent". If the send fails, the status is updated to "failed".

- **Updating Messages**:
  - After the message is sent, messages are fetched from the server to ensure the message state is up to date.

## `ChatComponent` Component

### Description

The `ChatComponent` is a React functional component that handles the chat interface. It uses the `useSignalRConnection` and `useHandleSendMessage` hooks to manage the SignalR connection and message sending. It also manages the message state, user information, and message thread scrolling.

### Features

1. **User Initialization**:
   - When the component mounts, the user ID is retrieved from local storage and stored in the state.

2. **SignalR Connection**:
   - The `useSignalRConnection` hook is used to initialize the SignalR connection and handle message reception and disconnection events.

3. **Message Sending**:
   - The `useHandleSendMessage` hook is used to handle message sending. When a message is sent, it is added to the message state with a "sending" status.

4. **Chat Initialization**:
   - The `initializeChat` function is used to fetch initial messages and set user information. This function is memoized using `useCallback` to avoid unnecessary re-renders.

5. **Automatic Scrolling**:
   - The component uses a reference to the message thread container to automatically scroll to the bottom whenever messages change.