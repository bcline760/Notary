import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { CertificatesModule } from './certificates/certificates.module';
import { ConfigurationModule } from './configuration/configuration.module';
import { KeyManagementModule } from './key-management/key-management.module';
import { EncryptionModule } from './encryption/encryption.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
    ]),
    CertificatesModule,
    ConfigurationModule,
    KeyManagementModule,
    EncryptionModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
