import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SessionRoutingModule } from './session-routing.module';
import { SessionComponent } from './session.component';
import { SigninComponent } from './signin/signin.component';


@NgModule({
  declarations: [SessionComponent, SigninComponent],
  imports: [
    CommonModule,
    SessionRoutingModule
  ]
})
export class SessionModule { }
