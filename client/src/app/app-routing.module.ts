import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: '', 
    runGuardsAndResolvers: 'always', // 
    canActivate: [AuthGuard], // canActivate: [AuthGuard] is a guard that prevents unauthorized access to the members page
    children: [
      { path: 'members', component: MemberListComponent,}, 
      { path: 'members/:username', component: MemberDetailComponent }, 
      { path: 'member/edit', component: MemberEditComponent , canDeactivate: [PreventUnsavedChangesGuard] },  // canDeactivate: [PreventUnsavedChangesGuard] is a guard that prevents the user from leaving the page without saving changes to the form 
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent }
    ]},
  /*
  { path: 'members', component: MemberListComponent, canActivate: [AuthGuard]}, // canActivate: [AuthGuard] is a guard that prevents unauthorized access to the members page
  { path: 'members/:id', component: MemberDetailComponent , canActivate: [AuthGuard] },
  { path: 'lists', component: ListsComponent },
  { path: 'messages', component: MessagesComponent },
  */
  { path: 'errors', component: TestErrorComponent },  
  { path: 'not-found', component: NotFoundComponent },  
  { path: 'server-error', component: ServerErrorComponent },  
  { path: '**', component: NotFoundComponent, pathMatch: 'full' }  // ** = wildcard

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
