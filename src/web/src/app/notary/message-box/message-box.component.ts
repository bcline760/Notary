import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-message-box',
  templateUrl: './message-box.component.html',
  styleUrls: ['./message-box.component.scss']
})
export class MessageBoxComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  @Input() public title: string;
  @Input() public message: string;
  @Input() public icon: 'Info' | 'Question' | 'Warning' | 'Error';
  @Input() public buttons: 'OK' | 'OKCancel' | 'YesNo' | 'YesNoCancel'
}
