import { outputAst } from '@angular/compiler';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
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
  registerForm: FormGroup =new FormGroup({}); // 
  constructor(private accoundService: AccountService,private toastr: ToastrService, 
    private fb:FormBuilder,
    private router:Router // this is used to navigate/redirect to another component after the registeration is successful
    ) 
    { }
  maxDate:Date=new Date();
  validationErrors:string[] | undefined;

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender:['male'],
      username:['', Validators.required],
      knownAs:['', Validators.required],
      dateOfBirth:['', Validators.required],
      city:['', Validators.required],
      country:['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required,this.matchPassword('password')]]
    });
    this.registerForm.controls['password'].valueChanges.subscribe(() => {
      this.registerForm.controls['confirmPassword'].updateValueAndValidity(); // this is to update the confirm password field validation when the password field is changed
    });

  }

  matchPassword(matchTo:string): ValidatorFn { // this is a custom validator function
      return (control: AbstractControl) => { // AbstractControl is the base class for FormControl, FormGroup, and FormArray
        return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true}; 
      } // return null if the password and confirm password are same else return notMatching: true 
      // control.parent?.get(matchTo)?.value is the value of the password field
      // control.value is the value of the confirm password field
  }
 
  register() {
    const dob= this.getDateOnly(this.registerForm.controls['dateOfBirth'].value); // this is to remove the time part from the date
    const values ={...this.registerForm.value, dateOfBirth: dob}; // replace the dateOfBirth value with the dob value
     //console.log(values);
    this.accoundService.register(values).subscribe(response => {
    this.router.navigateByUrl('/members'); // this is used to navigate/redirect to another component after the registeration is successful
    }
    , error => {
      console.log(error);
      this.validationErrors = error;
      //this.toastr.error(error.error);
    }
    );
     
  }

  cancel() {
    this.cancelRegister.emit(false);   // this is the output property from this child component to the parent component 
  }

  private getDateOnly(dob:string| undefined) {
    if (!dob) return ;
    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())).toISOString().slice(0,10); // this is to remove the time part from the date
  }
}
