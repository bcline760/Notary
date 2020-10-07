import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';

@Component({
    selector: 'app-root',
    styleUrls: ['./app.component.scss'],
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
    public showGateLayout: boolean = false;
    constructor(private router: Router, private activatedRoute: ActivatedRoute) {

    }

    ngOnInit(): void {
        this.router.events.subscribe(ev => {
            if (ev instanceof NavigationEnd) {
                if (this.activatedRoute.firstChild != null) {
                    this.showGateLayout = this.activatedRoute.firstChild.snapshot.data.showGateLayout === true;
                }
            }
        })
    }

    title = 'Notary';
}
