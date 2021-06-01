import { Component, OnInit } from "@angular/core";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
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
      nutritionValue: [""],
      proteins: [""],
      fats: [""],
      carbohydrates: [""],
      timeOfCooking: ["", [Validators.required]],
      portions: [""],
      difficulty: ["", [Validators.required]],
      category: ["", [Validators.required]],
      cuisine: ["", [Validators.required]],
      ingredients: this.fb.array([
        this.fb.group({
          ingredientName: ["", [Validators.required, Validators.minLength(2)]],
          ingredientQuantity: ["", [Validators.required]],
          unit: ["", [Validators.required]],
        }),
      ]),
      steps: this.fb.array([
        this.fb.group({
          stepDescription: ["", [Validators.required, Validators.minLength(5)]],
          stepImage: ["", [Validators.required]],
        }),
      ]),
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

  get ingredients() {
    return this.createRecipeForm.get("ingredients") as FormArray;
  }

  get steps() {
    return this.createRecipeForm.get("steps") as FormArray;
  }

  addIngredient() {
    this.ingredients.push(
      this.fb.group({
        ingredientName: ["", [Validators.required, Validators.minLength(2)]],
        ingredientQuantity: ["", [Validators.required]],
        unit: ["", [Validators.required]],
      })
    );
  }

  deleteIngredient(index) {
    this.ingredients.removeAt(index);
  }

  addStep() {
    this.steps.push(
      this.fb.group({
        stepDescription: ["", [Validators.required, Validators.minLength(5)]],
        stepImage: ["", [Validators.required]],
        stepImageSource: ["", [Validators.required]],
      })
    );
  }

  deleteStep(index) {
    this.steps.removeAt(index);
  }

  onMainImageFileChange(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.createRecipeForm.patchValue({
        mainImage: file,
      });
    }
  }

  onStepImageFileChange(event, index) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.steps.at(index).value.stepImage = file;
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
    this.createRecipeForm.value.ingredients.forEach((ingredient) => {
      recipe.ingredients.push({
        amount: ingredient.ingredientQuantity,
        name: ingredient.ingredientName,
        unit: ingredient.unit,
      });
    });

    recipe.difficulty = this.createRecipeForm.value.difficulty;

    recipe.stepsOfCooking = [];
    let counter = 1;
    this.createRecipeForm.value.steps.forEach((step) => {
      recipe.stepsOfCooking.push({
        description: step.stepDescription,
        imageName: step.stepImage.name,
        stepNumber: counter,
        image: null,
      });
      counter++;
    });

    recipe.servings = this.createRecipeForm.value.portions;

    recipe.nutritionValue = {
      carbohydrates: this.createRecipeForm.value.carbohydrates,
      fats: this.createRecipeForm.value.fats,
      proteins: this.createRecipeForm.value.proteins,
      value: this.createRecipeForm.value.nutritionValue,
    };

    recipe.cuisine = this.createRecipeForm.value.cuisine;

    formData.append("command", JSON.stringify(recipe));
    formData.append("mainImage", this.createRecipeForm.get("mainImage").value);
    counter = 1;
    this.createRecipeForm.value.steps.forEach((step) => {
      console.log(step.stepImage);
      formData.append("images", step.stepImage);
      counter++;
    });

    if (formData.getAll("images").length > 1) {
      this.recipeService
        .createRecipe(formData)
        .subscribe((response) => console.log(response));
    } else {
      this.recipeService
        .createRecipeWithOneImageInSteps(formData)
        .subscribe((response) => console.log(response));
    }
  }
}
