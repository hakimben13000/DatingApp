import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(), // BsDropdownModule menu added
    TabsModule.forRoot(), // TabsModule added,
    ToastrModule.forRoot( // ToastrModule added for message/error notification
      {positionClass: 'toast-bottom-right'}  
    ), // ToastrModule added
    NgxGalleryModule // module to show image gallery

  ],
  exports: [
    BsDropdownModule, 
    TabsModule,
    ToastrModule,
    NgxGalleryModule 
  ]
})
export class SharedModule { }
