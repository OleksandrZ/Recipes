import { Profile } from "./profile.module";

export interface Recipe{
    id: string,
    cuisine: string,
    author: Profile,
    title: string,
    difficulty: string,
    createdAt: string,
    updatedAt: string,
    timeOfCooking: string,
    images: [],
    servings: number,
    favorited: boolean,
    favoritesCount: number
}