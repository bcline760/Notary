import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SessionModule } from './session/session.module';

const routes: Routes = [
  { path: 'account', loadChildren: () => import('./account/account.module').then(m => m.AccountModule) },
  { path: 'certificates', loadChildren: () => import('./certificates/certificates.module').then(m => m.CertificatesModule) },
  { path: 'encryption', loadChildren: () => import('./encryption/encryption.module').then(m => m.EncryptionModule) },
  { path: 'home', loadChildren: () => import('./home/home.module').then(h => h.HomeModule) },
  { path: 'session', loadChildren: () => import('./session/session.module').then(s => s.SessionModule) },
  { path: 'settings', loadChildren: () => import('./settings/settings.module').then(s => s.SettingsModule) },
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
