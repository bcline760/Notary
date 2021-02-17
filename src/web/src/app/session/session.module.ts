import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SessionRoutingModule } from './session-routing.module';
import { SigninComponent } from './signin/signin.component';
import { SignoutComponent } from './signout/signout.component';
import { SessionComponent } from './session.component';


@NgModule({
  declarations: [SigninComponent, SignoutComponent, SessionComponent],
  imports: [
    CommonModule,
    SessionRoutingModule
  ]
})
export class SessionModule { }
