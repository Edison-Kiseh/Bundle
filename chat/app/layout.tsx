"use client";
import "../styles/globals.css";
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { initializeIcons, registerIcons } from '@fluentui/react';
import { FluentThemeProvider, DEFAULT_COMPONENT_ICONS } from '@azure/communication-react';

initializeIcons();
registerIcons({ icons: DEFAULT_COMPONENT_ICONS });

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
      <FluentThemeProvider>
        {children}
        <ToastContainer />
      </FluentThemeProvider>
      </body>
    </html>
  );
}