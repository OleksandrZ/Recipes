import { Image } from './image.module';

export interface Step {
  description: string;
  stepNumber: number;
  image: Image
}
