import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ExpensesService {

  constructor(private http: HttpClient) { }

  url = "http://localhost:5085";

  getAll() {
    return this.http.get(`${this.url}/Expense/GetAll`);
  }

  insetrExpence(body:any){
    return this.http.post(`${this.url}/Expense/Create`, body);
  }

  deleteItem(id: any){
    return this.http.delete(`${this.url}/Expense/Delete`, id);
  }
}
