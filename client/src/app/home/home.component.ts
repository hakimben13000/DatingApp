import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;
  baseUrl=environment.apiUrl;

  constructor() { }

  ngOnInit(): void {
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;  // registerMode is set to false when the cancel button is clicked in the register component 
  }

}
