import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { RouterModule } from '@angular/router';
import { SessionComponent } from './session.component';
import { FormsModule } from '@angular/forms';



@NgModule({
    declarations: [LoginComponent, LogoutComponent, SessionComponent],
    imports: [
        CommonModule,
        FormsModule,
        RouterModule.forChild([
            {
                path: "login",
                component: LoginComponent
            },
            {
                path: "logout",
                component: LogoutComponent
            }
        ])
    ],
    exports: [RouterModule]
})
export class SessionModule { }
