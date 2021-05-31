export interface Category{
    name: string;
}

export class Category implements Category{}

export interface CategoriesEnvelope{
    categories: Category[];
    categoryCount: number;
}
