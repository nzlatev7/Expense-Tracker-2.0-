import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  [x: string]: any;

  constructor(private http: HttpClient) { }

  url = "http://localhost:5085";

  loggedIn = false;

  private messageSource = new BehaviorSubject(false);
  logged = this.messageSource.asObservable();

  private messageSource1 = new BehaviorSubject(false);
  Admin = this.messageSource1.asObservable();
  
  register(body: any){
    return this.http.post(`${this.url}/User/Register`, body);
  }

  logIn(body: any){
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    })
    return this.http.post(`${this.url}/User/Login`, body, {headers: headers, responseType: "text"});
  }

  update(body: any){
    return this.http.put(`${this.url}/User/Update`, body);
  }

  getInfo(){
    return this.http.get(`${this.url}/User/GetInfo`);
  }

  deleteItem() {
    return this.http.delete(`${this.url}/User/Delete`);
  }

  checkToken(){
    const token = localStorage.getItem('token');
    if (token) {
      this.loggedIn = true;
      this.isAdmin();
    } else {
      this.loggedIn = false;
    }
    this.messageSource.next(this.loggedIn);
  }

  isAdmin(){
    let jwt = localStorage.getItem('token');
    jwt = JSON.stringify(jwt);
    let jwtData = jwt.split('.')[1];
    let decodedJwtJsonData = window.atob(jwtData);
    let decodedJwtData = JSON.parse(decodedJwtJsonData);
    let role = decodedJwtData.role;
    if (role === "Admin") {
      this.messageSource1.next(true);
    } else {
      this.messageSource1.next(false);
    }
  }
}
