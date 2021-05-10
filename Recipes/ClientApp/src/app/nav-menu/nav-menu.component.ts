import {
  ApplicationRef,
  Component,
  ElementRef,
  Inject,
  ViewChild,
} from "@angular/core";
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from "@angular/forms";
import { NgbModal, NgbModalConfig, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.css"],
  providers: [NgbModalConfig, NgbModal],
})
export class NavMenuComponent {
  loginForm: FormGroup;
  registerForm: FormGroup;
  fieldTextType: boolean;

  constructor(private modalService: NgbModal, private fb: FormBuilder) {
    this.createForm();
  }

  // Create login form and register form with validation
  private createForm() {
    this.loginForm = this.fb.group({
      loginEmail: ["", [Validators.required, Validators.email]],
      loginRememberMe: [""],
      loginPassword: [
        "",
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(
            "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"
          ),
        ],
      ],
    });

    this.registerForm = this.fb.group(
      {
        registerUsername: ["", [Validators.minLength(4), Validators.required]],
        registerEmail: ["", [Validators.required, Validators.email]],
        registerPassword: [
          "",
          [
            Validators.required,
            Validators.minLength(8),
            Validators.pattern(
              "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"
            ),
          ],
        ],
        registerConfirmPassword: [""],
        registerAcceptTerms: ["", Validators.required],
      },
      { validators: passwordMatchValidator }
    );
  }

  openModal(content) {
    if (this.modalService.hasOpenModals) {
      this.modalService.dismissAll();
    }

    let modalRef = this.modalService.open(content);

    modalRef.result
      .then((result) => {
        console.log("Closed with " + result);
        if(result === "Sign in"){

        }
        else if(result === "Sign up"){

        }
      }, (reason) => {
        console.log("Dismissed " + reason);
        this.loginForm.get("loginPassword").setValue("");
        this.registerForm.get("registerPassword").setValue("");
        this.registerForm.get("registerConfirmPassword").setValue("");
      })
      .catch((error) => {
        console.log(error);
      });
  }

  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }
}

export const passwordMatchValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const registerPassword = control.get("registerPassword");
  const registerConfirmPassword = control.get("registerConfirmPassword");

  return registerPassword &&
    registerConfirmPassword &&
    registerPassword.value === registerConfirmPassword.value
    ? null
    : { passwordMatch: true };
};
