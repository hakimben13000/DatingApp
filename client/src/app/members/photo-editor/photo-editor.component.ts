import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit{
    @Input() member: Member | undefined
    uploader:FileUploader | undefined;
    hasBaseDropZoneOver:boolean =false;
    baseUrl= environment.apiUrl;
    user : User | undefined;

    constructor(private accountService:AccountService,private memberService:MembersService){
        this.accountService.currentUser$.pipe(take(1)).subscribe({
          next: (user) => {
            if (user)
              this.user = user;
          }
        });
    }


    ngOnInit(): void {
      this.initializeUploader();
    }

    public fileOverBase(e:any):void {
      this.hasBaseDropZoneOver = e;
    }


    deletePhoto(photo:Photo){
      this.memberService.deletePhoto(photo.id).subscribe({
        next:()=> {
          if (this.member){
             this.member.photos = this.member.photos.filter(p => p.id !== photo.id);  // remove the photo from the array
          }
        }
      });
    }

    setMainPhoto(photo:Photo){
      this.memberService.setMainPhoto(photo.id).subscribe({
        next: () => {
          if (this.user && this.member){
            this.user.photoUrl = photo.url;
            this.accountService.setCurrentUser(this.user); // update the user in the local storage (to update the photo in the navbar)
            this.member.photoUrl = photo.url;            
            this.member.photos.forEach(p => {
              if (p.isMain) p.isMain = false;
              if (p.id === photo.id) p.isMain = true;
            })
          }
        }
      })

    }

    initializeUploader(){
      this.uploader = new FileUploader({
        url: this.baseUrl+'users/add-photo',
        authToken: 'Bearer '+this.user?.token, // add the token to the request header beacause it's don't use the interceptor
        isHTML5: true,
        allowedFileType: ['image'], // use all image type : png,jpeg .. 
        removeAfterUpload: true,
        autoUpload: false, // this mean we have to click to the button upload to upload photo 
        maxFileSize: 10*1024*1024 // max allowed by cloundinary
      });

      this.uploader.onAfterAddingFile = (file) => {
          file.withCredentials= false // 
      }

      this.uploader.onSuccessItem= (item,response,status,headers) => {
           if (response){
              const photo= JSON.parse(response);
              this.member?.photos.push(photo);
              if (photo.isMain && this.user && this.member){
                this.user.photoUrl = photo.url;
                this.accountService.setCurrentUser(this.user); // update the user in the local storage (to update the photo in the navbar)
                this.member.photoUrl = photo.url;
              }
           }
      }

    }


}
