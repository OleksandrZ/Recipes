import { Profile } from "./profile.module";

export interface Recipe{
    id: string,
    cuisine: string,
    author: Profile,
    title: string,
    difficulty: string,
    createAt: string,
    updatedAt: string,
    timeOfCooking: string,
    images: [],
    servings: number,
    favorited: boolean,
    favoritesCount: number
}