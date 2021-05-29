import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { CuisinesEnvelope, Recipe, RecipeEnvelope } from "../modules";
import { environment } from "src/environments/environment.prod";
import { CategoriesEnvelope, Category } from "../modules/category.module";

@Injectable({
  providedIn: "root",
})
export class RecipeService {
  constructor(private http: HttpClient) {}

  getRecipes(): Observable<RecipeEnvelope> {
    return this.http.get<RecipeEnvelope>(environment.api_url + "recipe/all");
  }

  getRecipeById(id: string): Observable<Recipe> {
    return this.http.get<Recipe>(environment.api_url + "recipe/" + id);
  }

  getAllCategories(): Observable<CategoriesEnvelope> {
    return this.http.get<CategoriesEnvelope>(
      environment.api_url + "category/all"
    );
  }

  getAllCuisines(): Observable<CuisinesEnvelope> {
    return this.http.get<CuisinesEnvelope>(environment.api_url + "cuisine/all");
  }

  getAllDifficulties() : Observable<string[]>{
    return this.http.get<string[]>(environment.api_url + "difficulty/all");
  }
}
