import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.scss']
})
export class SessionComponent implements OnInit {

  public showFullLayout: boolean = false;
  constructor(private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.router.events.subscribe(ev => {
      if (ev instanceof NavigationEnd) {
        if (this.activatedRoute.firstChild !== null) {
          this.showFullLayout = this.activatedRoute.firstChild.snapshot.data.showFullLayout !== false;
        }
      }
    });
  }

}
