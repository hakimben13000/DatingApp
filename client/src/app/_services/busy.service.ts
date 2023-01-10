import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0; // as  number of requests  that are busy (taking place) , we increase it

  constructor(private spinnerService:NgxSpinnerService) { }

  busy() {
    this.busyRequestCount++; // increase the number of busy requests
    this.spinnerService.show(undefined, { // show the spinner
      type: 'line-scale-party',
      bdColor: 'rgba(255,255,255,0)', // background color
      color: '#333333' // color of the spinner
    });
  }

    Idle(){
      this.busyRequestCount--; // decrease the number of busy requests
      if (this.busyRequestCount <= 0) { // if there are no busy requests then hide the spinner
        this.busyRequestCount = 0;
        this.spinnerService.hide(); // hide the spinner
      }
    }
}
