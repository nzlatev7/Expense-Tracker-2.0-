import { Component, OnInit } from '@angular/core';
import { ExpensesService } from '../core/services/expenses.service';

@Component({
  selector: 'app-add-item',
  templateUrl: './add-item.component.html',
  styleUrls: ['./add-item.component.scss']
})
export class AddItemComponent implements OnInit {

  constructor(private http: ExpensesService) { }

  bool: boolean = false;

  private id: number = 0;

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

  update() {
    const body = {
      "id": this.id,
      "name": this.form.name,
      "type": this.form.type,
      "date": this.form.date,
      "amount": this.form.amount
    }
    this.http.update(body).subscribe({
      next: () => this.getExpences(),
      error: (error) => console.log(error)
    });
    this.cancel();
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

  changeForm(body: any){
    this.bool = true;
    this.form.amount = body.amount;
    this.form.name = body.name;
    this.form.date = body.date;
    this.form.type = body.type;
    this.id = body.id;
  }

  cancel(){
    this.bool = false;
    this.form = {
      name: '',
      type: '',
      date: '',
      amount: 0
    }
    this.id = 0;
  }
}