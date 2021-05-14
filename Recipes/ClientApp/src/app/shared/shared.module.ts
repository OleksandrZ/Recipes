import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShowAuthDirective } from './show-auth.directive';



@NgModule({
  declarations: [
    ShowAuthDirective
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ShowAuthDirective
  ]
})
export class SharedModule { }
