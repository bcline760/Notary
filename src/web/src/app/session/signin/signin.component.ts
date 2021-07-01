import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Signin } from 'src/app/models/signin.model';
import { SessionService } from 'src/app/services/session.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent implements OnInit {
  public loginForm: FormGroup;
  public signinModel: Signin;
  public showError: boolean = false;
  private _redirectUrl: string = '/';
  constructor(private router: Router, private activatedRoute: ActivatedRoute, private session: SessionService) {
    this.signinModel = new Signin();

    this.loginForm = new FormGroup({
      password: new FormControl(this.signinModel.password, [
        Validators.required,
        Validators.minLength(6)
      ]),
      persist: new FormControl(this.signinModel.persist),
      username: new FormControl(this.signinModel.username, [
        Validators.required,
        Validators.minLength(4)
      ])
    });
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.pipe(first()).subscribe(s => {
      if (s['redirect'] !== null) {
        this._redirectUrl = s['redirect'];
      }
    });
  }

  onSubmit(): void {

  }

  get username() { return this.loginForm?.get('username'); }
  get password() { return this.loginForm?.get('password'); }
}
