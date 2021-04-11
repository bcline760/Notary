import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap'
import { MessageBoxButton } from 'src/model/message-box-button.enum';

@Component({
  selector: 'app-message-box',
  templateUrl: './message-box.component.html',
  styleUrls: ['./message-box.component.scss']
})
export class MessageBoxComponent implements OnInit {

  private _modalRef: NgbModalRef;
  @ViewChild('modal') private modalContent: TemplateRef<MessageBoxComponent>
  constructor(private modalSvc: NgbModal) { }

  ngOnInit(): void {
  }

  /**
   * open - Opens the message box
   */
  public open(): Promise<boolean> {
    return new Promise<boolean>(resolve => {
      this._modalRef = this.modalSvc.open(this.modalContent);
      this._modalRef.result.then(resolve, resolve);
    })
  }

  /**
   * close - Closes the message box
   */
  public close(button: string): MessageBoxButton {
    this._modalRef.close();

    let clickedButton: MessageBoxButton = MessageBoxButton.None;
    switch (button) {
      case 'OK':
        this.onClose(MessageBoxButton.OK);
        break;
      case 'Cancel':
        this.onClose(MessageBoxButton.Cancel);
        break;
      case 'Yes':
        this.onClose(MessageBoxButton.Yes);
        break;
      case 'No':
        this.onClose(MessageBoxButton.No);
        break;
    }

    return clickedButton;
  }

  public onClose: (button: MessageBoxButton) => void;

  @Input() public title: string;
  @Input() public message: string;
  @Input() public icon: 'Info' | 'Question' | 'Warning' | 'Error';
  @Input() public buttons: 'OK' | 'OKCancel' | 'YesNo' | 'YesNoCancel'
}
