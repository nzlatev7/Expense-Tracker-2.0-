import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  url = "http://localhost:5085";

  register(body: any){
    return this.http.post(`${this.url}/User/Register`, body);
  }

  logIn(body: any){
    return this.http.post(`${this.url}/User/Login`, body);
  }

  update(body: any){
    return this.http.put(`${this.url}/User/Update`, body);
  }

  getAll(){
    return this.http.get(`${this.url}/User/GetAll`);
  }

  deleteItem(id: any) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    })
    return this.http.delete(`${this.url}/User/Delete`, { headers: headers, body: id });
  }

}
