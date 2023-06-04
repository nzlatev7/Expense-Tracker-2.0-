import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }

  url = "http://localhost:5085";

  getUsers(body: any){
    let queryParams = {
      'pageNumber': body
    };
    return this.http.get(`${this.url}/Admin/GetAllStepByStep`, {params:queryParams});
  } 

}
