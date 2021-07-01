import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from './services/auth-guard.service';

const routes: Routes = [
  {
    path: 'sessions', loadChildren: () => import('./session/session.module').then(m => m.SessionModule),
    data: {
      showFullLayout: true
    }
  },
  {
    path: 'accounts', loadChildren: () => import('./account/account.module').then(m => m.AccountModule), canActivate: [AuthGuardService],
    data: {
      showFullLayout: false
    }
  },
  {
    path: '', loadChildren: () => import('./entry/entry.module').then(m => m.EntryModule), pathMatch: 'full', canActivate: [AuthGuardService],
    data: {
      showFullLayout: false
    }
  },
  {
    path: 'certificates', loadChildren: () => import('./certificate/certificate.module').then(m => m.CertificateModule), canActivate: [AuthGuardService],
    data: {
      showFullLayout: false
    }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
