export const useApiError = (form?: any) => {
  const handleApiError = (error: any, setStatus?: Function) => {
    if (error.response) {
      if (error.response.status === 400) {
        //validation error
        let errorMessages = "";
        Object.entries(error.response.data.errors).forEach((error) => {
          if (Array.isArray(error[1])) {
            error[1].forEach((x) => (errorMessages += x + "\n"));
          } else errorMessages += error[1] + "\n";
        });
        setStatus?.(errorMessages);
      } else if (error.response.status === 409) {
        //business error
        console.log("business error", error.response);
        setStatus?.(error.response.data.Message);
      } else {
        console.log(error.response.data);
        console.log("here", error.response.status);
        // navigate("/404");
      }
    }
  };

  return {
    handleApiError,
  };
};
