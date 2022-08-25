interface ImageProps {
  src: string;
  alt?: string;
  className?: string;
  width?: string;
  height?: string;
}

export const Image = (props: ImageProps) => {
  const { src, alt, className, width, height } = props;

  return (
    <img
      className={className}
      width={width}
      height={height}
      src={`data:image/png;base64, ${src}`}
      alt={alt}
    />
  );
};
