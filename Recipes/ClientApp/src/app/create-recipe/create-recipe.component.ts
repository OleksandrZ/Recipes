import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { RecipeService } from "../core";
import { CategoriesEnvelope, CuisinesEnvelope, Recipe } from "../core/modules";
import { Category } from "./../core/modules/category.module";
import { Ingredient } from "./../core/modules/ingredient.module";
import { Step } from "./../core/modules/step.module";

@Component({
  selector: "app-create-recipe",
  templateUrl: "./create-recipe.component.html",
  styleUrls: ["./create-recipe.component.css"],
})
export class CreateRecipeComponent implements OnInit {
  createRecipeForm: FormGroup;

  difficulties: string[];
  categoryEnvelope: CategoriesEnvelope;
  cuisineEnvelope: CuisinesEnvelope;

  constructor(private fb: FormBuilder, private recipeService: RecipeService) {}

  ngOnInit(): void {
    this.createRecipeForm = this.fb.group({
      title: ["", [Validators.required, Validators.minLength(4)]],
      description: ["", [Validators.required, Validators.minLength(10)]],
      mainImage: ["", [Validators.required]],
      mainImageSource: ["", [Validators.required]],
      nutritionValue: [""],
      proteins: [""],
      fats: [""],
      carbohydrates: [""],
      timeOfCooking: ["", [Validators.required]],
      portions: [""],
      difficulty: ["", [Validators.required]],
      category: ["", [Validators.required]],
      cuisine: ["", [Validators.required]],
      ingredientName: ["", [Validators.required, Validators.minLength(2)]],
      ingredientQuantity: ["", [Validators.required]],
      unit: ["", [Validators.required]],
      stepDescription: ["", [Validators.required, Validators.minLength(5)]],
      stepImage: ["", [Validators.required]],
      stepImageSource: ["", [Validators.required]],
    });

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

  onMainImageFileChange(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.createRecipeForm.patchValue({
        mainImageSource: file,
      });
    }
  }

  onStepImageFileChange(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.createRecipeForm.patchValue({
        stepImageSource: file,
      });
    }
  }

  createRecipe() {
    const formData = new FormData();
    let recipe = new Recipe();
    recipe.categories = [];
    recipe.categories.push({
      name: this.createRecipeForm.value.category,
    });

    recipe.description = this.createRecipeForm.value.description;
    recipe.title = this.createRecipeForm.value.title;

    recipe.ingredients = [];
    let ingredient = new Ingredient();
    ingredient.amount = this.createRecipeForm.value.ingredientQuantity;
    ingredient.name = this.createRecipeForm.value.ingredientName;
    ingredient.unit = this.createRecipeForm.value.unit;

    recipe.ingredients.push(ingredient);

    recipe.difficulty = this.createRecipeForm.value.difficulty;

    recipe.stepsOfCooking = [];
    let step = new Step();
    step.description = this.createRecipeForm.value.stepDescription;
    step.stepNumber = 1;
    step.imageName = this.createRecipeForm.value.stepImage.substr(
      this.createRecipeForm.value.stepImage.lastIndexOf("\\") + 1
    );
    recipe.stepsOfCooking.push(step);

    recipe.servings = this.createRecipeForm.value.portions;

    recipe.nutritionValue = {
      carbohydrates: this.createRecipeForm.value.carbohydrates,
      fats: this.createRecipeForm.value.fats,
      proteins: this.createRecipeForm.value.proteins,
      value: this.createRecipeForm.value.nutritionValue,
    };

    recipe.cuisine = this.createRecipeForm.value.cuisine;

    formData.append("command", JSON.stringify(recipe));
    formData.append(
      "mainImage",
      this.createRecipeForm.get("mainImageSource").value,
      this.createRecipeForm.value.mainImage.substr(
        this.createRecipeForm.value.mainImage.lastIndexOf("\\") + 1
      )
    );
    formData.append(
      "image",
      this.createRecipeForm.get("stepImageSource").value
    );

    this.recipeService
      .createRecipeWithOneImageInSteps(formData)
      .subscribe((response) => console.log(response));
  }
}
