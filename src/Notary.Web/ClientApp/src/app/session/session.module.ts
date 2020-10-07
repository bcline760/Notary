import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { RouterModule } from '@angular/router';
import { SessionComponent } from './session.component';



@NgModule({
    declarations: [LoginComponent, LogoutComponent, SessionComponent],
    imports: [
        CommonModule,
        RouterModule.forChild([
            {
                path: '',
                pathMatch: 'full',
                children: [
                    {
                        path: "login",
                        component: LoginComponent
                    },
                    {
                        path: "logout",
                        component: LogoutComponent
                    }
                ],
                component: SessionComponent
            }
        ])
    ],
    exports: [RouterModule]
})
export class SessionModule { }
