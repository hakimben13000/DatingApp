import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {
  
  }
// add the token to the request header if the user is logged in 
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.accountService.currentUser$.pipe(take(1)).subscribe({ // pipe the observable to take only one value  
      next: (user) => {
            if(user){ // if the user is logged in, add the token to the request header
              request = request.clone({ // clone the request and add the token to the headers 
                setHeaders: {
                  Authorization: `Bearer ${user.token}` // add the token to the header
                }
              }
              );
            }
          }
    });
    return next.handle(request); // return the request to the next handler
  }
}
