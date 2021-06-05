import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: 'sessions', loadChildren: () => import('./session/session.module').then(m => m.SessionModule) },
  { path: 'accounts', loadChildren: () => import('./account/account.module').then(m => m.AccountModule) },
  { path: '', loadChildren: () => import('./entry/entry.module').then(m => m.EntryModule), pathMatch: 'full' },
  { path: 'certificates', loadChildren: () => import('./certificate/certificate.module').then(m => m.CertificateModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
