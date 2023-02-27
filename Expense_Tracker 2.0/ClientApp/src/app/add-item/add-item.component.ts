import { Component, OnInit } from '@angular/core';
import { ExpensesService } from '../services/expenses.service';

@Component({
  selector: 'app-add-item',
  templateUrl: './add-item.component.html',
  styleUrls: ['./add-item.component.scss']
})
export class AddItemComponent implements OnInit {

  constructor(private http: ExpensesService) { }

  ngOnInit(): void {
    this.getExpences();
  }

  form = {
    name: '',
    type: '',
    date: '',
    amount: 0
  }

  items: any = [];

  getExpences() {
    this.http.getAll()
      .subscribe({
        next: (resp) => this.items = resp,
        error: (error) => console.log(error)
      });
  }


  insert() {
    const body = {
      "name": this.form.name,
      "type": this.form.type,
      "date": this.form.date,
      "amount": this.form.amount
    }

    this.http.insetrExpence(body).subscribe({
      next: () => this.getExpences(),
      error: (error) => console.log(error)
    });
  }

  deleteExpense(id: number) {
    const request = {
      "id": id
    }

    this.http.deleteItem(request)
      .subscribe({
        next: () => this.getExpences(),
        error: (error) => console.log(error)
      });
  }
}