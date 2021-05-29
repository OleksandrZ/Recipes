import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Recipe, RecipeEnvelope } from "../modules";
import { environment } from "src/environments/environment.prod";
import { Comment } from "./../modules/comment.module";
import { catchError } from "rxjs/operators";

@Injectable({
  providedIn: "root",
})
export class CommentService {
  constructor(private http: HttpClient) {}

  createComment(comment: CommandComment): Observable<Comment> {
    return this.http.post<Comment>(
      environment.api_url + "comments/create",
      comment
    );
  }
}

export class CommandComment {
  constructor(recipeId: string, body: string) {
    this.recipeId = recipeId;
    this.body = body;
  }
  recipeId: string;
  body: string;
}
