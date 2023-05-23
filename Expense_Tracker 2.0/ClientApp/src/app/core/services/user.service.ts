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
  currentMessage = this.messageSource.asObservable();
  
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

  deleteItem(id: any) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    })
    return this.http.delete(`${this.url}/User/Delete`, { headers: headers, body: id });
  }

  checkToken(){
    const token = localStorage.getItem('token');
    if (token) {
      this.loggedIn = true;
    } else {
      this.loggedIn = false;
    }
    this.messageSource.next(this.loggedIn);
  }
}
