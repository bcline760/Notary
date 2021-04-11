import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { MessageBoxComponent } from 'src/app/notary/message-box/message-box.component';
import { Account } from 'src/contract/account.contract';
import { MessageBoxButton } from 'src/model/message-box-button.enum';
import { AccountService } from 'src/service/account.service';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss']
})
export class AccountsComponent implements OnInit, AfterViewInit {

  private _accountList: Observable<Account[]>;
  private _selectedAccount: Account;
  private _selectedSlug: string;

  @ViewChild(MessageBoxComponent) private confirmDelete: MessageBoxComponent;

  constructor(private accountSvc: AccountService) { }

  ngOnInit(): void {
    this._loadAccounts();
  }

  ngAfterViewInit(): void {
    this.confirmDelete.onClose = this.onClose;
  }

  public openAccount(slug: string) {

  }

  public deleteAccount(slug: string) {
    this._selectedSlug = slug;
    this.confirmDelete.open();
  }

  private onClose(button: MessageBoxButton) {
    if (button === MessageBoxButton.Yes) {
      console.log('Deleting account..')
      this._deleteAccount(this._selectedSlug);
    }
  }

  private _loadAccountDetail(slug: string): void {
    //First check the current list of object. Save on API calls.
    this.accountList.pipe(first()).subscribe(a => {
      a.find((value, index) => {
        if (value.slug === slug) {
          this._selectedAccount = value;
        }
      })
    });

    // Get the object from the server if it is not found in the array list
    if (this._selectedAccount === null) {
      this.accountSvc.get(slug)?.pipe(first()).subscribe(a => {
        this._selectedAccount = a;
      });
    }
  }

  private _loadAccounts(): void {
    this._accountList = this.accountSvc.getAll();
  }

  private _deleteAccount(slug: string): void {
    this._loadAccountDetail(slug);

  }

  get accountList() { return this._accountList; }
  get selectedAccount() { return this._selectedAccount; }
}
