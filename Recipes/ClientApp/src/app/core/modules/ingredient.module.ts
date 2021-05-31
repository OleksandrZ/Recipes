export interface Ingredient {
  id: string;
  name: string;
  unit: string;
  amount: number;
}

export class Ingredient implements Ingredient{}
