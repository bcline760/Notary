import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/service/auth-guard.service';
import { HomeComponent } from './home.component';

const routes: Routes = [
    {
        path: '',
        canActivate: [AuthGuardService],
        component: HomeComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HomeRoutingModule { }
