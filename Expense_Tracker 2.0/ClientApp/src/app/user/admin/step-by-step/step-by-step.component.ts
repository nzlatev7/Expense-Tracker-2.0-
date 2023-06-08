import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AdminService } from 'src/app/core/services/admin.service';

@Component({
  selector: 'app-step-by-step',
  templateUrl: './step-by-step.component.html',
  styleUrls: ['./step-by-step.component.scss']
})
export class StepByStepComponent implements OnInit {
  constructor(private admin: AdminService,
    private router: Router) { }

  users: any = [];

  currentPage: number = 1;

  isFirstPage: boolean = true;
  isLastPage: boolean = false;

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.admin.getUsers(this.currentPage).subscribe({
      next: (resp: any) => {
        this.users = resp;
        if (resp.length < 10 || resp.length == 0) {
          this.isLastPage = true;
        } else {
          this.isLastPage = false;
        }
      },
      error: error => console.log(error)
    });
  }

  nextPage(): void {
    this.currentPage++;
    this.loadUsers();
    this.pageCheck();
  }

  previousPage(): void {
    this.currentPage--;
    this.loadUsers();
    this.pageCheck();
  }

  pageCheck(): void {
    if (this.currentPage == 1) {
      this.isFirstPage = true;
    } else {
      this.isFirstPage = false;
    }
  }

  delete(id: number): void {
    this.admin.geleteUser(id).subscribe({
      next: () => this.loadUsers(),
      error: error => console.log(error)
    })
  }

  update(user: any): void {
    this.router.navigate(["admin/userUpdate"]);
    this.admin.passUserInfo(user);
  }

}
