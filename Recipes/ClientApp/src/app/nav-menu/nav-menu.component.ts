import { Component, ElementRef, Inject, ViewChild } from "@angular/core";
import { NgbModal, NgbModalConfig } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.css"],
  providers: [NgbModalConfig, NgbModal],
})
export class NavMenuComponent {
  constructor(config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = true;
  }

  openModal(content) {
    this.modalService.dismissAll();
    let modalRef = this.modalService.open(content);

    modalRef.closed.subscribe((result) => {
      console.log(result);
    });

    modalRef.dismissed.subscribe((reason) => {
      console.log(reason);
    });
  }
}
