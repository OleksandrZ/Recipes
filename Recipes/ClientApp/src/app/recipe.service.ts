import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Recipe } from './core/modules/recipe.module';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from './../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  recipes: Recipe[];
  recipeUrl = "recipe/all";

  constructor(private http: HttpClient) { }

  getRecipes(): Observable<Recipe[]>{
    return this.http.get<Recipe[]>(environment.api_url + this.recipeUrl);
  }
}
