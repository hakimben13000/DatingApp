import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;
  baseUrl="https://localhost:5001/api/";

  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  getUsers(){
    this.http.get(this.baseUrl+'users/').subscribe( {
      next: response => this.users = response,
      error: err => console.log(err),
      complete: () => console.log('Request completed')
      
    });
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;  // registerMode is set to false when the cancel button is clicked in the register component 
   
  }

}
