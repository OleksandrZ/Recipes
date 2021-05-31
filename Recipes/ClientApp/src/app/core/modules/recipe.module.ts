import { TimeSpan } from "./timespan.module";
import { NutritionValue } from "./nutritionValue.module";
import { Ingredient } from "./ingredient.module";
import { Step } from "./step.module";
import { Category } from "./category.module";
import { Comment } from "./comment.module";
import { Image } from "./image.module";

export interface Recipe {
  id: string;
  description: string;
  cuisine: string;
  author: string;
  title: string;
  difficulty: string;
  date: string;
  timeOfCooking: TimeSpan;
  mainImage: Image;
  servings: number;
  favorited: boolean;
  favoritesCount: number;
  likes: number;
  nutritionValue: NutritionValue;
  ingredients: Ingredient[];
  stepsOfCooking: Step[];
  comments: Comment[];
  categories: Category[];
}

export class Recipe implements Recipe{

}

export interface RecipeEnvelope {
  recipeCount: number;
  recipes: Recipe[];
}

export interface CuisinesEnvelope {
  cuisines: string[];
  cuisineCount: number;
}
