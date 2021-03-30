import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/service/auth-guard.service';

import { EncryptionComponent } from './encryption.component';

const routes: Routes = [
  {
    path: 'encryption',
    component: EncryptionComponent,
    canActivate: [AuthGuardService],
    data: { showFullLayout: true }
  }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EncryptionRoutingModule { }
