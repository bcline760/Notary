import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/service/auth-guard.service';


const routes: Routes = [
  {
    path: '',
    canActivate: [AuthGuardService],
    loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule),
    data: { showFullLayout: true }
  },
  {
    path: 'certificates',
    canActivate: [AuthGuardService],
    loadChildren: () => import('./certificates/certificates.module').then(m => m.CertificatesModule),
    data: { showFullLayout: true }
  },
  {
    path: 'session',
    loadChildren: () => import('./session/session.module').then(m => m.SessionModule),
    data: { showFullLayout: false }
  },
  {
    path: 'dashboard',
    canActivate: [AuthGuardService],
    loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule),
    data: { showFullLayout: true }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
