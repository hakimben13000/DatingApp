import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor { // ControlValueAccessor is an interface that we need to implement to make our component work with the form control 
  @Input() label= '';
  @Input() type= 'text';
  
  constructor(@Self() public ngControl: NgControl ) // Self is used to get the instance of the component itself 
  { 
    this.ngControl.valueAccessor = this; // this is used to set the value accessor of the ngControl to the component itself (TextInputComponent class)
  }

  writeValue(obj: any): void {

  }

  registerOnChange(fn: any): void {

  }

  registerOnTouched(fn: any): void {

  }

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }


}
