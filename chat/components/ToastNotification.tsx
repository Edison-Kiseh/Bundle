import * as React from "react";
import {
  useId,
  Toaster,
  useToastController,
  Toast,
  ToastTitle,
  ToastBody,
} from "@fluentui/react-components";

export const ToastNotification = () => {
  const toasterId = useId("toaster");
  return (
      <Toaster toasterId={toasterId} />
  );
};

export const useToast = () => {
  const toasterId = useId("toaster");
  const { dispatchToast } = useToastController(toasterId);

  const notify = (title: string, body: string, intent: "success" | "error") =>
    dispatchToast(
      <Toast>
        <ToastTitle>{title}</ToastTitle>
        <ToastBody>{body}</ToastBody>
      </Toast>,
      { intent }
    );

  return { notify };
};