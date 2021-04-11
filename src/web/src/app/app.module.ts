import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Provider } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from './layout/layout.module';

import { SessionService } from 'src/service/session.service';
import { TokenService } from 'src/service/token.service';
import { CertificateService } from 'src/service/certificate.service';
import { AuthGuardService } from 'src/service/auth-guard.service';

import { CertificatesModule } from './certificates/certificates.module';
import { EncryptionModule } from './encryption/encryption.module';
import { SessionModule } from './session/session.module';
import { HomeModule } from './home/home.module';
import { HomeComponent } from './home/home.component';
import { LoadingInterceptor } from 'src/intercept/loading.interceptor';
import { ErrorInterceptor } from 'src/intercept/error.interceptor';
import { JwtInterceptor } from 'src/intercept/jwt.interceptor';
import { SettingsModule } from './settings/settings.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

const providers: Provider[] = [
  SessionService,
  TokenService,
  CertificateService,
  AuthGuardService,
  {
    provide: HTTP_INTERCEPTORS,
    useClass: LoadingInterceptor,
    multi: true
  },
  {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
  },
  {
    provide: HTTP_INTERCEPTORS,
    useClass: JwtInterceptor,
    multi: true
  }
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
    HttpClientModule,
    LayoutModule,
    CertificatesModule,
    EncryptionModule,
    SessionModule,
    HomeModule,
    EncryptionModule,
    SettingsModule,
    AppRoutingModule,
    NgbModule
  ],
  bootstrap: [AppComponent],
  providers: providers
})
export class AppModule { }
