import { TimeSpan } from "./timespan.module";
import { NutritionValue } from './nutritionValue.module';
import { Ingredient } from './ingredient.module';
import { Step } from './step.module';
import { Category } from './category.module';

export interface Recipe {
  id: string;
  cuisine: string;
  author: string;
  title: string;
  difficulty: string;
  date: string;
  timeOfCooking: TimeSpan;
  images: any[];
  servings: number;
  favorited: boolean;
  favoritesCount: number;
  likes: number;
  NutritionValue: NutritionValue;
  ingredients: Ingredient[];
  stepsOfCooking: Step[];
  comments: any[];
  categories: Category[];
}

export interface RecipeEnvelope {
  recipeCount: number;
  recipes: Recipe[];
}
