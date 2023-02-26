import { Component, OnInit } from '@angular/core';
import { ExpensesService } from '../servises/expenses.service';

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

  items: item[] = [];

  getExpences() {
    this.http.getAll().subscribe({
      next(resp) {
        console.log('Response: ', resp);
      },
      error(err) {
        console.log('Error: ', err);
      }
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
      next(resp) {
        console.log('Response: ', resp);
      },
      error(err) {
        console.log('Error: ', err);
      }
    });
  }
}

type item = {
  name: string,
  type: string,
  date: string,
  amount: number
}