import { Component } from "@angular/core";
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from "@angular/forms";
import {
  NgbModal,
  NgbModalConfig,
  NgbModalRef,
} from "@ng-bootstrap/ng-bootstrap";
import { finalize, first } from "rxjs/operators";
import { AuthService } from "../core/services/auth.service";

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
  busy: boolean;

  constructor(
    private modalService: NgbModal,
    private fb: FormBuilder,
    private authService: AuthService
  ) {
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
      .then(
        (result) => {
          console.log("Closed with " + result);
        },
        (reason) => {
          console.log("Dismissed " + reason);
          this.loginForm.get("loginPassword").setValue("");
          this.registerForm.get("registerPassword").setValue("");
          this.registerForm.get("registerConfirmPassword").setValue("");
        }
      )
      .catch((error) => {
        console.log(error);
      });
  }

  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }

  login() {
    if (this.loginForm.invalid) {
      return;
    }

    let email = this.loginForm.value.loginEmail;
    let password = this.loginForm.value.loginPassword;
    let rememberMe: boolean = this.loginForm.value.loginRememberMe;

    this.busy = true;

    this.authService
      .login(email, password, rememberMe)
      .pipe(first())
      .subscribe({
        next: () => {
          console.log("Ok");
          this.modalService.dismissAll();
        },
        error: (error) => {
          console.log("Error");
          console.log(error);
          this.modalService.dismissAll();
        },
      });
  }

  logout(){
    this.authService.logout();
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
