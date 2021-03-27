import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/service/auth-guard.service';
import { HomeModule } from './home/home.module';
import { SessionModule } from './session/session.module';


const routes: Routes = [
  {
    path: 'certificates',
    loadChildren: () => import('./certificates/certificates.module').then(m => m.CertificatesModule),
    data: { showFullLayout: true }
  },
  {
    path: 'encryption',
    loadChildren: () => import('./encryption/encryption.module').then(m => m.EncryptionModule),
    data: { showFullLayout: true }
  },
  {
    path: 'home',
    loadChildren: () => import('./home/home.module').then(h => HomeModule),
    data: { showFullLayout: true }
  },
  {
    path: 'session',
    loadChildren: () => import('./session/session.module').then(s => SessionModule),
    data: { showFullLayout: false }
  },
  {
    path: '',
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
