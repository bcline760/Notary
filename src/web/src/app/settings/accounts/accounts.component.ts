import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { Account } from 'src/contract/account.contract';
import { AccountService } from 'src/service/account.service';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss']
})
export class AccountsComponent implements OnInit {

  private _accountList: Observable<Account[]>;
  private _selectedAccount: Account;

  constructor(private accountSvc: AccountService) { }

  ngOnInit(): void {
    this._loadAccounts();
  }

  public openAccount(slug: string) {

  }

  private _loadAccountDetail(slug: string): void {
    this.accountSvc.get(slug)?.pipe(first()).subscribe(a => {
      this._selectedAccount = a;
    });
  }

  private _loadAccounts(): void {
    this._accountList = this.accountSvc.getAll();
  }

  get accountList() { return this._accountList; }
  get selectedAccount() { return this._selectedAccount; }
}
