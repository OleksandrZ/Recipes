import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Recipe, RecipeEnvelope } from "../modules";
import { environment } from "src/environments/environment.prod";

@Injectable({
  providedIn: "root",
})
export class RecipeService {
  recipes: Recipe[];

  constructor(private http: HttpClient) {}

  getRecipes(): Observable<RecipeEnvelope> {
    return this.http.get<RecipeEnvelope>(environment.api_url + "recipe/all");
  }

  getRecipeById(id: string): Observable<Recipe> {
    return this.http.get<Recipe>(environment.api_url + "recipe/" + id);
  }
}
