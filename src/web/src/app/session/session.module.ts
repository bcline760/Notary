import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SessionRoutingModule } from './session-routing.module';
import { SigninComponent } from './signin/signin.component';
import { SignoutComponent } from './signout/signout.component';
import { SessionComponent } from './session.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [SigninComponent, SignoutComponent, SessionComponent],
  imports: [
    CommonModule,
    FormsModule,
    SessionRoutingModule
  ]
})
export class SessionModule { }
