import { outputAst } from '@angular/compiler';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
 // @Input() usersfromhomeComponent: any;  // this is the input property from the parent component to this child component
  @Output() cancelRegister = new EventEmitter(); // this is the output property from this child component to the parent component
  
  model: any = {};
  passwordNotMatch: boolean = false;
  constructor(private accoundService: AccountService,private toastr: ToastrService) { }

  ngOnInit(): void {
  }
 
  register() {
    if (this.model.password != this.model.confirmpassword) {
      console.log('passwords do not match');
      this.passwordNotMatch = true;
    }
    else{
      
      this.passwordNotMatch = false;
      this.accoundService.register(this.model).subscribe(response => {
        console.log(response);
        this.cancel();
      }
      , error => {
        console.log(error);
        this.toastr.error(error.error);
      }
      );
    }
     
  }

  cancel() {
    this.cancelRegister.emit(false);   // this is the output property from this child component to the parent component 
  }
}
