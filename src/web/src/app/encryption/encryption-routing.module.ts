import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/service/auth-guard.service';

import { EncryptionComponent } from './encryption.component';

const routes: Routes = [{ path: '', component: EncryptionComponent, canActivate: [AuthGuardService] }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EncryptionRoutingModule { }
