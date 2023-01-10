import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl= environment.apiUrl;
  constructor(private http:HttpClient) { }
  members: Member[] = [];

  getMembers(){
    if (this.members.length > 0) return of(this.members); // si on a déjà les membres, on les retourne , of est un observable car on doit retourner un observable
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map((members) =>{
        this.members = members; // on stocke les membres dans la variable members dans le cas où members n'est pas encore appellé
        return members;
      })
    ); // getHttpOptions pour ajouter le token d'authentification est inclut dans intercepteur
   //  return this.http.get<Member[]>(this.baseUrl + 'users', this.getHttpOptions()); // getHttpOptions est inclut dans intercepteur
  }

  getMember(username:string){
    const member = this.members.find(x => x.userName === username);
    if (member !== undefined) return of(member); // si on a déjà le membre, on le retourne , of est un observable car on doit retourner un observable

   // return this.http.get<Member>(this.baseUrl + 'users/' + username, this.getHttpOptions());  // getHttpOptions est inclut dans intercepteur
   return this.http.get<Member>(this.baseUrl + 'users/' + username); // getHttpOptions pour ajouter le token d'authentification est inclut dans intercepteur
  }

  updateMember(member:Member){
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(
        () => {
          const index = this.members.indexOf(member); // on trouve l'index du membre à mettre à jour
          this.members[index] =  {...this.members[index],...member } // on met à jour le membre dans le tableau de membres en utilisant le spread operator ,
          // on utilise spread operator car  member est un objet et on ne veut pas modifier l'objet member mais en créer un nouveau avec les nouvelles valeurs de member et les anciennes valeurs de this.members[index]
        }
      )
    ); 
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
