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
	commentsAmount: number;
	createCommentForm: FormGroup;

	page = 1;
	itemsPerPage = 6;
	pageSize: number;

	constructor(
		private route: ActivatedRoute,
		private recipeService: RecipeService,
		private commentService: CommentService,
		private fb: FormBuilder
	) { }

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
			this.commentsAmount = this.recipe.comments.length;
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
				console.log(comment);
				this.createCommentForm.reset();
			});
	}

	onPageChange(page) {
		this.pageSize = this.itemsPerPage * (page - 1);
	}
}
