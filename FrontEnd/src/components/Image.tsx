import React from "react";

interface ImageProps {
  src: string;
  alt?: string;
  className?: string;
}

export const Image = (props: ImageProps) => {
  const { src, alt, className } = props;

  return (
    <img
      className={className}
      src={`data:image/png;base64, ${src}`}
      alt={alt}
    />
  );
};
