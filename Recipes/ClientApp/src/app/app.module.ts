import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { RouterModule } from "@angular/router";

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { HomeComponent } from "./home/home.component";
import { RecipeComponent } from "./recipe/recipe.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { NgbAlertModule, NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { CoreModule } from "./core/core.module";
import { SharedModule } from "./shared/shared.module";
import { CreateRecipeComponent } from "./create-recipe/create-recipe.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RecipeComponent,
    CreateRecipeComponent,
  ],
  imports: [
    NgbAlertModule,
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CoreModule,
    SharedModule,
    HttpClientModule,
    RouterModule.forRoot(
      [
        { path: "", component: HomeComponent, pathMatch: "full" },
        { path: "recipe/:id", component: RecipeComponent },
        // { path: 'fetch-data', component: FetchDataComponent },
      ],
      { relativeLinkResolution: "legacy" }
    ),
    BrowserAnimationsModule,
    NgbModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
