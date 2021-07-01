import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from '../services/auth-guard.service';
import { EntryComponent } from './entry.component';

const routes: Routes = [{
  path: '',
  component: EntryComponent,
  canActivate: [AuthGuardService],
  data: {
    showFullLayout: false
  }
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EntryRoutingModule { }
