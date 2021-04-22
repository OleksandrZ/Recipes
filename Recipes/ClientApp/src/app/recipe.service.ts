import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Recipe, RecipeEnvelope } from './core/modules/recipe.module';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from './../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  recipes: Recipe[];
  recipeUrl = "recipe/all";

  constructor(private http: HttpClient) { }

  getRecipes(): Observable<RecipeEnvelope>{
    return this.http.get<RecipeEnvelope>(environment.api_url + this.recipeUrl);
  }
}
