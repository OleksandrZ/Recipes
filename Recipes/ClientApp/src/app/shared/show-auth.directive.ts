import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from './../core/services/auth.service';

@Directive({
  selector: '[appShowAuth]'
})
export class ShowAuthDirective {

  constructor(
    private templateRef: TemplateRef<any>,
    private authService: AuthService,
    private viewContainer: ViewContainerRef
  ) {}

  condition: boolean;

  ngOnInit() {
    this.authService.isAuthenticated.subscribe(
      (isAuthenticated) => {       
        if (isAuthenticated && this.condition || !isAuthenticated && !this.condition) {
          this.viewContainer.createEmbeddedView(this.templateRef);
        } else {
          this.viewContainer.clear();
        }
      }
    );
  }

  @Input() set appShowAuth(condition: boolean) {
    this.condition = condition;
  }

}
