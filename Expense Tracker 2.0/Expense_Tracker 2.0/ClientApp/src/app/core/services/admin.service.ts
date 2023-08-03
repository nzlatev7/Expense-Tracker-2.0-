import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, NgZone } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient,
    private _zone: NgZone) { }

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
    console.log(requestBody);
    return this.http.put(`${this.url}/Admin/Update`, requestBody);
  }

  private info$ = new BehaviorSubject<any>({});
  changeInfo$ = this.info$.asObservable();

  passUserInfo(info: any): void {
    this.info$.next(info);
  }

  // getSSE(): Observable<T> {
  //   let token = localStorage.getItem('token');
  //   const headers = new HttpHeaders({
  //     'Authorization': `Bearer ${token}`
  //   }) 


  //   const eventSource = new EventSource(`${this.url}/Admin/StreamUsers`);

  //   return new Observable(observer => {
  //     eventSource.onmessage = event => {
  //       const messageData: MessageData = JSON.parse(event.data);
  //       observer.next(messageData);
  //     };
  //   });
  // }

  getSSE(): Observable<any> {

    let token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    })
    return new Observable(observer => {
      const eventSource = new EventSource(`${this.url}/Admin/StreamUsers`);
      eventSource.onmessage = event => {
        this._zone.run(() => {
          observer.next(event);
        });
      };
      eventSource.onerror = error => {
        this._zone.run(() => {
          observer.error(error);
        });
      };
    });
  }
}
