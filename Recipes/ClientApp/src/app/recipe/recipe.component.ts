import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { RecipeService } from "../core";
import { Recipe } from "../core/modules";
import {
  CommandComment,
  CommentService,
} from "./../core/services/comment.service";

@Component({
  selector: "app-recipe",
  templateUrl: "./recipe.component.html",
  styleUrls: ["./recipe.component.css"],
})
export class RecipeComponent implements OnInit {
  recipe: Recipe;
  public recipeLoaded: boolean;
  private recipeId: string;
  createCommentForm: FormGroup;
  constructor(
    private route: ActivatedRoute,
    private recipeService: RecipeService,
    private commentService: CommentService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.recipeId = this.route.snapshot.paramMap.get("id");
    this.recipeLoaded = false;
    this.getRecipe(this.recipeId);

    this.createCommentForm = this.fb.group({
      body: ["", [Validators.required, Validators.minLength(2)]],
    });
  }

  getRecipe(recipeId: string) {
    this.recipeService.getRecipeById(recipeId).subscribe((recipe) => {
      this.recipe = recipe;
      console.log(this.recipe);

      this.recipeLoaded = true;
    });
  }

  createComment() {
    if (this.createCommentForm.invalid) {
      return;
    }

    let body = this.createCommentForm.value.body;
    console.log(body);
    
    this.commentService
      .createComment(new CommandComment(this.recipeId, body))
      .subscribe((comment) => {
        this.recipe.comments.push(comment);
      });
  }
}
