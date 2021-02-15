import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent implements OnInit {

  public username: string | null;
  public password: string | null;

  constructor() {
    this.username = null;
    this.password = null;
  }

  ngOnInit(): void {
  }

  onLogin(): void {

  }
}
