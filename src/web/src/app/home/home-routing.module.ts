import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/service/auth-guard.service';
import { HomeDisplayComponent } from './home-display/home-display.component';
import { HomeComponent } from './home.component';

const routes: Routes = [
  {
    path: 'home',
    canActivate: [AuthGuardService],
    component: HomeComponent,
    data: { showFullLayout: true },
    children: [
      {
        path: '',
        component: HomeDisplayComponent,
        pathMatch: 'full'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
