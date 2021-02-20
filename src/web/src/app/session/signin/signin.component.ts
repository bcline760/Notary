import { Component, OnInit } from '@angular/core';
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
  }

  onSubmit(): void {
    if (this.model == null) {
      return;
    }

    const credentials: Credentials = {
      key: this.model.username,
      secret: this.model.password,
      persistant: this.model.persist
    };

    this.session.signIn(credentials).pipe(first()).subscribe(s => {
      if (s) {
        this.router.navigateByUrl(this._redirectUrl);
      }
    });
  }
}
