import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from '../services/auth-guard.service';
import { CertificateComponent } from './certificate.component';

const routes: Routes = [
  {
    path: 'certificates',
    component: CertificateComponent,
    canActivate: [AuthGuardService],
    data: {
      showFullLayout: false
    }
  }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CertificateRoutingModule { }
