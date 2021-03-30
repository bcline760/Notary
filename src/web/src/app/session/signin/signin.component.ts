import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first, map } from 'rxjs/operators';
import { Credentials } from 'src/contract/credentials.contract';
import { LoginRequest } from 'src/model/login-request.model';
import { SessionService } from 'src/service/session.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent implements OnInit {

  public model: LoginRequest;
  public showError: boolean = false;
  public loginForm: FormGroup;

  private _redirectUrl: string = '/';

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private session: SessionService) {
    if (!this.model) {
      this.model = new LoginRequest();
    }
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams
      .pipe(first())
      .subscribe(s => {
        if (s['redirect'] !== null) {
          this._redirectUrl = s['redirect'];
        }
      })


    if (this.session.isValidSession()) {
      this.router.navigateByUrl(this._redirectUrl);
    }

    this.loginForm = new FormGroup({
      username: new FormControl(this.model.username, [
        Validators.required,
        Validators.minLength(4)
      ]),
      password: new FormControl(this.model.password, [
        Validators.required,
        Validators.minLength(8)
      ]),
      persist: new FormControl(this.model.persist)
    })
  }

  onSubmit(): void {
    const form = this.loginForm.value;

    const credentials: Credentials = {
      key: form.username,
      secret: form.password,
      persistant: form.persist
    };

    this.session.signIn(credentials).pipe(first()).subscribe(s => {
      if (s) {
        this.router.navigateByUrl(this._redirectUrl);
      } else {
        this.showError = true;
      }
    });
  }

  get username() { return this.loginForm.get('username'); }

  get password() { return this.loginForm.get('password'); }
}
