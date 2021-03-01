import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { LoadingService } from 'src/service/loading.service';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss']
})
export class LoadingComponent implements OnInit, OnDestroy {
  loading: boolean = false;
  loadingSub: Subscription;

  constructor(private loadingService: LoadingService) { }

  ngOnDestroy(): void {
    if (this.loadingSub) {
      this.loadingSub.unsubscribe();
    }
  }

  ngOnInit(): void {
    this.loadingSub = this.loadingService.loadingStatus.pipe(
      debounceTime(200)
    ).subscribe((v) => {
      this.loading = v;
    });
  }
}
