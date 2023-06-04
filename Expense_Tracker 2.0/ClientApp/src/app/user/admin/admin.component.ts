import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/core/services/admin.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {

  constructor(private admin: AdminService) { }

  users: any = [];
  
  currentPage = 1;
  firstPageLoaded = true;
  lastPageLoaded = false;

  ngOnInit(): void {
    this.loadUsers();
  } 
  
  loadUsers(){
    this.admin.getUsers(this.currentPage).subscribe({
      next: resp => this.users = resp,
      error: error => console.log(error)
    });
  }

  nextPage(){
    this.currentPage++;
    this.loadUsers();
    this.pageCheck();
  }

  previousPage(){
    this.currentPage--;
    this.loadUsers();
    this.pageCheck();
  }

  pageCheck(){
    if (this.currentPage == 1) {
      this.firstPageLoaded = true;
    } else {
      this.firstPageLoaded = false;
    }
  }


}
