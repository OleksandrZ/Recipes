import { Image } from "./image.module";

export interface Step {
  description: string;
  stepNumber: number;
  image: Image;
}

export class Step implements Step{
  description: string;
  stepNumber: number;
  image: Image;
  imageName: string;
}
