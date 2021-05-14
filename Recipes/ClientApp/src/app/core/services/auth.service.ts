import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { map } from "rxjs/operators";
import { ApplicationUser } from "./../models/application-user";
import { environment } from "src/environments/environment";

@Injectable({ providedIn: "root" })
export class AuthService {
  private userSubject: BehaviorSubject<ApplicationUser>;
  public user: Observable<ApplicationUser>;
  private readonly apiUrl = `${environment.api_url}user`;

  constructor(private router: Router, private http: HttpClient) {
    this.userSubject = new BehaviorSubject<ApplicationUser>(null);
    this.user = this.userSubject.asObservable();
  }

  public get userValue(): ApplicationUser {
    return this.userSubject.value;
  }

  login(email: string, password: string, rememberMe: boolean) {
    return this.http
      .post<any>(
        `${this.apiUrl}/login`,
        { email, password, rememberMe },
        { withCredentials: true }
      )
      .pipe(
        map((user) => {
          this.userSubject.next(user);
          console.log(this.userValue);
          
          this.startRefreshTokenTimer();
          return user;
        })
      );
  }

  logout() {
    this.http
      .post<any>(`${this.apiUrl}/logout`, {}, { withCredentials: true })
      .subscribe();
    this.stopRefreshTokenTimer();
    this.userSubject.next(null);
    this.router.navigate(["/"]);
  }

  refreshToken() {
    return this.http
      .post<any>(`${this.apiUrl}/refresh-token`, {}, { withCredentials: true })
      .pipe(
        map((user) => {
          this.userSubject.next(user);
          this.startRefreshTokenTimer();
          return user;
        })
      );
  }

  // helper methods

  private refreshTokenTimeout;

  private startRefreshTokenTimer() {
    // parse json object from base64 encoded jwt token
    const jwtToken = JSON.parse(atob(this.userValue.jwtToken.split(".")[1]));

    // set a timeout to refresh the token a minute before it expires
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - 60 * 1000;
    this.refreshTokenTimeout = setTimeout(
      () => this.refreshToken().subscribe(),
      timeout
    );
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
