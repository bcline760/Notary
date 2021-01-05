import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

    constructor() {
        this.password = '';
        this.emailAddress = '';
        this.persist = false;
    }

    ngOnInit() {
    }

    public emailAddress: string | null;
    public password: string | null;
    public persist: boolean;
}
