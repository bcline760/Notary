import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/service/auth-guard.service';
import { CertificateModule } from './certificate/certificate.module';
import { EncryptionModule } from './encryption/encryption.module';
import { HomeModule } from './home/home.module';
import { SessionModule } from './session/session.module';


const routes: Routes = [
  {
    path: 'certificates',
    canActivate: [AuthGuardService],
    loadChildren: () => import('./certificate/certificate.module').then(c => CertificateModule)
  },
  {
    path: 'encryption',
    canActivate: [AuthGuardService],
    loadChildren: () => import('./encryption/encryption.module').then(e => EncryptionModule)
  },
  {
    path: 'home',
    canActivate: [AuthGuardService],
    loadChildren: () => import('./home/home.module').then(h => HomeModule)
  },
  {
    path: 'session',
    canActivate: [AuthGuardService],
    loadChildren: () => import('./session/session.module').then(s => SessionModule)
  },
  {
    path: '',
    canActivate: [AuthGuardService],
    redirectTo: '/home',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(
      routes,
      { enableTracing: true }
    )
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
