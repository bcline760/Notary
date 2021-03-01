import { Component, OnInit } from '@angular/core';
import { AuthenticatedUser } from 'src/contract/authenticated-user.contract';
import { LayoutHeader } from 'src/model/layout-header.model';
import { SessionService } from 'src/service/session.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  private _model: LayoutHeader;

  constructor(private sessionService: SessionService) {
    this._model = new LayoutHeader();
  }

  ngOnInit(): void {
    const authenticatedUser: AuthenticatedUser | null = this.sessionService.currentAuthenticatedUser;

    if (authenticatedUser) {
      const fullName: string = `${authenticatedUser.fName} ${authenticatedUser.lName}`.trim();
      this._model.fullName = fullName;
      this._model.accountSlug = authenticatedUser.accountSlug;
      this._model.role = authenticatedUser.role;
    }
  }


  public get model() { return this._model; }
}
