import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }

  url = "http://localhost:5085";

  getUsers(body: any) {
    let queryParams = {
      'pageNumber': body
    };
    return this.http.get(`${this.url}/Admin/GetAllStepByStep`, { params: queryParams });
  }

  geleteUser(id: number) {
    const body = {
      'id': id
    }
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.delete(`${this.url}/Admin/Delete`, { headers, body });
  }

  updateUser(requestBody: any) {
    // const body = requestBody;
    // console.log(body)
    // const headers = new HttpHeaders({
    //   'Content-Type': 'application/json'
    // });
    return this.http.put(`${this.url}/Admin/Update`, requestBody);
  }

  private info$ = new BehaviorSubject<any>({});
  changeInfo$ = this.info$.asObservable();

  passUserInfo(info: any): void {
    this.info$.next(info);
  }

}
