import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl= environment.apiUrl;
  constructor(private http:HttpClient) { }

  getMembers(){
    return this.http.get<Member[]>(this.baseUrl + 'users'); // getHttpOptions pour ajouter le token d'authentification est inclut dans intercepteur
   //  return this.http.get<Member[]>(this.baseUrl + 'users', this.getHttpOptions()); // getHttpOptions est inclut dans intercepteur
  }

  getMember(username:string){
   // return this.http.get<Member>(this.baseUrl + 'users/' + username, this.getHttpOptions());  // getHttpOptions est inclut dans intercepteur
   return this.http.get<Member>(this.baseUrl + 'users/' + username); // getHttpOptions pour ajouter le token d'authentification est inclut dans intercepteur
  }

  
  /*getHttpOptions(){ // ce code est remplacé par le code dans le intercepteur pour éviter d'ajouter le token à chaque appel
    const userString =localStorage.getItem('user');
    if (!userString) return;
    const user = JSON.parse(userString);
    return {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + user.token
      })
    }
  } */
}
