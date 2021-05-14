import { Component, OnInit } from "@angular/core";
import { Recipe, RecipeEnvelope } from "./../core/modules/recipe.module";
import { RecipeService } from "./../recipe.service";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})
export class HomeComponent implements OnInit {
  recipeEnvelope: RecipeEnvelope;
  recipesLoaded = false;
  page = 1;
  itemsPerPage = 9;
  pageSize: number;
  constructor(private recipeService: RecipeService) {}

  ngOnInit() {
    this.getRecipes();
  }

  getRecipes(): void {
    this.recipeService.getRecipes().subscribe((recipeEnvelope) => {
      this.recipeEnvelope = recipeEnvelope;
      this.recipesLoaded = true;
    });
  }

  onPageChange(page){
    this.pageSize = this.itemsPerPage*(page - 1);
  }
}
