export interface Category{
    name: string;
}

export interface CategoriesEnvelope{
    categories: Category[];
    categoryCount: number;
}