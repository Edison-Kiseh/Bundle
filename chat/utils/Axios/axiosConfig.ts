import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(
  (config) => {
    return config;
  },
  (error) => {
    return Promise.reject(new Error(error.message));
  }
);


axiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (axios.isAxiosError(error)) {
      if (error.response) {
        let errorMessage = 'An error occurred. Please try later.';
        switch (error.response.status) {
          case 401:
            errorMessage = 'Unauthorized. Please log in.';
            break;
          case 403:
            errorMessage = 'Forbidden. You do not have permission.';
            break;
          case 404:
            errorMessage = 'Resource not found.';
            break;
          case 409:
            errorMessage = 'Conflict. Value already used.';
            break;
          case 500:
            errorMessage = 'Internal server error. Please try later.';
            break;
        }
        return Promise.reject(new Error(errorMessage));
      } else {
        return Promise.reject(new Error('Network error. Please check your connection.'));
      }
    } else {
      return Promise.reject(new Error('An unexpected error occurred.'));
    }
  }
);

export default axiosInstance;