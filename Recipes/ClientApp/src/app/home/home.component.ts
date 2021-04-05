import { Component } from '@angular/core';
import { Recipe } from './../core/modules/recipe.module';
import { RecipeService } from './../recipe.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  recipes: Recipe[];
  constructor(private recipeService: RecipeService){

  }

  ngOnInit(){
    this.getRecipes();
  }

  getRecipes() : void {
    this.recipeService.getRecipes()
        .subscribe(recipes => this.recipes = recipes);
  }
}
