export const getImageFormData = (image?: File) => {
  const form = new FormData();
  if (image) form.append("file", image);

  return form;
};
