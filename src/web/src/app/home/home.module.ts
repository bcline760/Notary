import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeComponent } from './home.component';
import { HomeRoutingModule } from './home-routing.module';
import { HomeDisplayComponent } from './home-display/home-display.component';
import { SessionService } from 'src/service/session.service';
import { CertificateService } from 'src/service/certificate.service';

@NgModule({
  declarations: [HomeComponent, HomeDisplayComponent],
  imports: [
    CommonModule,
    HomeRoutingModule
  ],
  providers: [SessionService, CertificateService]
})
export class HomeModule { }
