import { Component, ElementRef, Inject, ViewChild } from "@angular/core";
import { NgbModal, NgbModalConfig } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.css"],
  providers: [NgbModalConfig, NgbModal],
})
export class NavMenuComponent {
  @ViewChild("loginPosition") loginPosition: ElementRef<HTMLElement>;
  @ViewChild("visibleLogin") visibleLogin: ElementRef<HTMLElement>;

  @ViewChild("registerPosition") registerPosition: ElementRef<HTMLElement>;
  @ViewChild("visibleRegister") visibleRegister: ElementRef<HTMLElement>;

  constructor(config: NgbModalConfig, private modalService: NgbModal) {
    // customize default values of modals used by this component tree
  }

  openSignIn(content) {
    this.modalService.dismissAll();

    this.visibleLogin.nativeElement.style.display = "block";
    let modalRef = this.modalService.open(content, {
      container: this.loginPosition.nativeElement,
    });

    modalRef.closed.subscribe((result) => {
      this.visibleLogin.nativeElement.style.display = "none";
      console.log(result);
    });

    modalRef.dismissed.subscribe((reason) => {
      this.visibleLogin.nativeElement.style.display = "none";
      console.log(reason);
    });
  }

  openSignUp(content) {
    this.modalService.dismissAll();

    this.visibleRegister.nativeElement.style.display = "block";
    let modalRef = this.modalService.open(content, {
      container: this.registerPosition.nativeElement,
    });

    modalRef.closed.subscribe((result) => {
      this.visibleRegister.nativeElement.style.display = "none";
      console.log(result);
    });

    modalRef.dismissed.subscribe((reason) => {
      this.visibleRegister.nativeElement.style.display = "none";
      console.log(reason);
    });
  }
}
