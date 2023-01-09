import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any= {}


  constructor(public accountService:AccountService, private router: Router,private toastr: ToastrService) { }

  ngOnInit(): void {
      this.model.username="lisa";
      this.model.password="Pa$$w0rd";
  }

 

  login(){
    this.accountService.login(this.model).subscribe({
     
       next: () =>  this.router.navigateByUrl('/members'), // go to members page
      /*next: response => {
        //console.log(response);
      },*/
      /*error: err => {
        this.toastr.error(err.error,'Error',{
          timeOut: 1000,
        });
      }*/
    });
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}
