import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Provider } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from './layout/layout.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FlexLayoutModule } from '@angular/flex-layout';
import { SessionService } from 'src/service/session.service';
import { TokenService } from 'src/service/token.service';
import { CertificateService } from 'src/service/certificate.service';
import { CertificatesModule } from './certificates/certificates.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthGuardService } from 'src/service/auth-guard.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MatCommonModule } from '@angular/material/core';

const providers: Provider[] = [
  SessionService,
  TokenService,
  CertificateService,
  AuthGuardService
]

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
    FontAwesomeModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    MatCommonModule,
    HttpClientModule,
    CertificatesModule,
    LayoutModule,
    AppRoutingModule
  ],
  providers: providers,
  bootstrap: [AppComponent]
})
export class AppModule { }
