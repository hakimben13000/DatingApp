import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit{
  @ViewChild('editForm') editForm: NgForm | undefined; // ViewChild is used to get the form  form #editForm="ngForm" in the html
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) { // HostListener is used to get the event before the user leaves the page
    if (this.editForm?.dirty) { // if the form is dirty (has been changed) then ask the user if they want to continue or not
      $event.returnValue = true; 
    }
  }

  member:Member | undefined;
  user:User | null = null;

  constructor(private accountService:AccountService,private memberService:MembersService, private toastr: ToastrService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe((usr) => this.user = usr!);
  }
  
  ngOnInit() {
    this.loadMember();
  }

  loadMember() {
        if (!this.user) return;
        this.memberService.getMember(this.user.userName!).subscribe((member) => this.member = member);
  }

  updateMember(){
    console.log(this.member);
    this.memberService.updateMember(this.member!).subscribe(() => {
      this.toastr.success("Profile updated successfully");
      this.editForm?.reset(this.member); // reset the form to the current member values after the update is successful to avoid the dirty form warning and the save button to be enabled
    }
    );
    
  }

}
