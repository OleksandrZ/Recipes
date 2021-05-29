import { Component, OnInit } from "@angular/core";
import { RecipeService } from "../core";
import { CategoriesEnvelope } from "../core/modules/category.module";
import {
  CuisinesEnvelope,
  Recipe,
  RecipeEnvelope,
} from "./../core/modules/recipe.module";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})
export class HomeComponent implements OnInit {
  recipeEnvelope: RecipeEnvelope;
  categoryEnvelope: CategoriesEnvelope;
  cuisineEnvelope: CuisinesEnvelope;
  difficulties: string[];
  recipesLoaded = false;
  page = 1;
  itemsPerPage = 9;
  pageSize: number;
  constructor(private recipeService: RecipeService) {}

  ngOnInit() {
    this.getRecipes();

    this.recipeService.getAllCategories().subscribe((categoryEnvelope) => {
      this.categoryEnvelope = categoryEnvelope;
    });

    this.recipeService.getAllCuisines().subscribe((cuisineEnvelope) => {
      this.cuisineEnvelope = cuisineEnvelope;
    });

    this.recipeService.getAllDifficulties().subscribe((difficulties) => {
      this.difficulties = difficulties;
    });
  }

  getRecipes(): void {
    this.recipeService.getRecipes().subscribe((recipeEnvelope) => {
      this.recipeEnvelope = recipeEnvelope;
      this.recipesLoaded = true;
    });
  }

  onPageChange(page) {
    this.pageSize = this.itemsPerPage * (page - 1);
  }
}
