import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { CertificatesModule } from './certificates/certificates.module';
import { ConfigurationModule } from './configuration/configuration.module';
import { KeyManagementModule } from './key-management/key-management.module';
import { EncryptionModule } from './encryption/encryption.module';
import { SessionModule } from './session/session.module';
import { JwtModule } from '@auth0/angular-jwt';

import { AuthGuardService } from '../service/auth-guard.service';
import { SessionService } from '../service/session.service';
import { CertificateService } from '../service/certificate.service';
import { TokenService } from '../service/token.service';


export function fnTokenGetter() {
    return localStorage.getItem("access_token");
}

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        JwtModule.forRoot({
            config: {
                throwNoTokenError: false,
                tokenGetter: fnTokenGetter,
                whitelistedDomains: ['localhost'],
            }
        }),
        FormsModule,
        CertificatesModule,
        ConfigurationModule,
        KeyManagementModule,
        EncryptionModule,
        BrowserAnimationsModule,
        SessionModule,
        RouterModule.forRoot([
            {
                path: '',
                pathMatch: 'full',
                canActivate: [AuthGuardService],
                redirectTo: '/certificates'
            },
            {
                path: 'certificates',
                loadChildren: () => import('./certificates/certificates.module').then(c => CertificatesModule),
                data: { showGateLayout: false },
                canActivate: [AuthGuardService]
            },
            {
                path: 'configuration',
                loadChildren: () => import('./configuration/configuration.module').then(c => ConfigurationModule),
                data: { showGateLayout: false },
                canActivate: [AuthGuardService]
            },
            {
                path: 'encryption',
                loadChildren: () => import('./encryption/encryption.module').then(e => EncryptionModule),
                data: { showGateLayout: false },
                canActivate: [AuthGuardService]
            },
            {
                path: 'key-management',
                loadChildren: () => import('./key-management/key-management.module').then(k => KeyManagementModule),
                data: { showGateLayout: false },
                canActivate: [AuthGuardService]
            },
            {
                path: 'session',
                loadChildren: () => import('./session/session.module').then(s => SessionModule),
                data: { showGateLayout: true }
            }
        ]),
    ],
    providers: [HttpClient, SessionService, CertificateService, AuthGuardService, TokenService],
    bootstrap: [AppComponent]
})
export class AppModule { }
