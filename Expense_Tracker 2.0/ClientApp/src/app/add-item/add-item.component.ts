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

  items: any = [];

  getExpences() {
    this.http.getAll()
    .subscribe({
      next: (resp) => {this.items = resp, console.log(resp)},
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
      next(resp) {
        console.log('Response: ', resp);
      },
      error(err) {
        console.log('Error: ', err);
      }
    });

    this.getExpences();

  }

  deleteExpense(id: any){
    this.http.deleteItem(id)
    .subscribe({
      next: (resp) => console.log(resp),
      error: (error) => console.log(error)
    });
    
    this.getExpences();
  }
}