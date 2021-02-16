import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  public showFullLayout: boolean = false;
  private _routerSub: Subscription | null = null;

  constructor(private router: Router, private activatedRoute: ActivatedRoute) {

  }

  ngOnDestroy(): void {
    if (this._routerSub) {
      this._routerSub.unsubscribe();
    }
  }

  ngOnInit(): void {
    this.router.events.subscribe(ev => {
      if (ev instanceof NavigationEnd) {
        if (this.activatedRoute.firstChild !== null) {
          this.showFullLayout = this.activatedRoute.firstChild.snapshot.data.showFullLayout !== false;
        }
      }
    })
  }
  title = 'Notary';
}
