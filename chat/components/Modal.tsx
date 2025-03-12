"use client";
import { removeUserFromChat } from "../utils/chatUtils";
import { useRouter } from "next/navigation";
import { toast } from 'react-toastify';

interface ModalProps {
  onClose: () => void;
}

function Modal({ onClose }: ModalProps) {
  const router = useRouter(); 

  const handleClick = async () => {
    const username = localStorage.getItem("username");

    if (username) {
      try
      {
        await removeUserFromChat(username);
        toast.success("You have left the chat.");
        localStorage.removeItem("username");

        router.push("/");
      }
      catch {
        toast.error("Failed to leave the chat, please try later.");
      }

    }
    else{
      console.error("No username stored in local storage");
    }
  };

  return (
    <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full flex items-center justify-center" style={{ zIndex: 9999 }}>
      <div className="p-8 border w-96 shadow-lg rounded-md bg-white">
        <div className="text-center">
          <h3 className="text-2xl font-bold text-gray-900">Leave chat</h3>
          <div className="mt-2 px-7 py-3">
            <p className="text-lg text-gray-500">Are you sure you want to leave the chat?</p>
          </div>
          <div className="flex justify-around mt-4">
            <button
              className="px-4 py-2 w-[100px] bg-red-500 text-white text-base font-medium rounded-md shadow-sm hover:bg-gray-400 focus:outline-none focus:ring-2 focus:ring-gray-300"
              onClick={() => {
                handleClick();
                onClose();
              }}
            >
              Yes
            </button>
            <button
              onClick={onClose}
              className="px-4 py-2 w-[100px] bg-blue-500 text-white text-base font-medium rounded-md shadow-sm hover:bg-gray-400 focus:outline-none focus:ring-2 focus:ring-gray-300"
            >
              No
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Modal;