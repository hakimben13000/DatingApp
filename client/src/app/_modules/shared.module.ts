import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from "ngx-spinner";
import { FileUploadModule } from 'ng2-file-upload';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(), // BsDropdownModule menu added
    TabsModule.forRoot(), // TabsModule added,
    ToastrModule.forRoot( // ToastrModule added for message/error notification
      {positionClass: 'toast-bottom-right'}  
    ), // ToastrModule added
    NgxGalleryModule, // module to show image gallery,
    NgxSpinnerModule.forRoot({ // module to show waiting spinner while loading
      type: 'line-scale-party'
    }),
    FileUploadModule // module to upload files ( ng2-file-upload)

  ],
  exports: [
    BsDropdownModule, 
    TabsModule,
    ToastrModule,
    NgxGalleryModule ,
    NgxSpinnerModule ,
    FileUploadModule
  ]
})
export class SharedModule { }
