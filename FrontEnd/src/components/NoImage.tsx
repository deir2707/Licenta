import NoImageSvg from "./NoImageSvg.svg";

interface NoImageProps {
  className?: string;
}

export const NoImage = (props: NoImageProps) => {
  return <img src={NoImageSvg} alt="missing" className={props.className} />;
};
