import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { RecipeService } from "../core";
import { Recipe } from "../core/modules";

@Component({
  selector: "app-recipe",
  templateUrl: "./recipe.component.html",
  styleUrls: ["./recipe.component.css"],
})
export class RecipeComponent implements OnInit {
  recipe: Recipe;
  public recipeLoaded: boolean;
  constructor(
    private route: ActivatedRoute,
    private recipeService: RecipeService
  ) {}

  ngOnInit() {
    const recipeId = this.route.snapshot.paramMap.get("id");
    this.recipeLoaded = false;
    this.getRecipe(recipeId);
  }

  getRecipe(recipeId: string) {
    this.recipeService.getRecipeById(recipeId).subscribe((recipe) => {
      this.recipe = recipe;
      console.log(this.recipe);
      
      this.recipeLoaded = true;
    });
  }
}
