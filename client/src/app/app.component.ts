import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { ModalComponent, ModalService } from './components/modal';
import { HttpClient } from '@angular/common/http';
import { NgControlStatus } from '@angular/forms';
import { User } from './interfaces/user';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, ModalComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  title: string = 'A';

  users: User[] = [];


  constructor(private modalService: ModalService, private http: HttpClient) { }

  // happens after the constructor
  ngOnInit(): void {
    //
    this.http.get<User[]>('https://localhost:8080/api/users').subscribe({
      next: data => this.users = data,
      error: error => console.error('Error:', error),
      complete: () => console.log("Request has completed")
    })
  }


  // Modal Configuration
  openModal(text_id: string, product_id: any) {
    this.modalService.open(text_id)
  }

  closeModal(text_id: string) {
    this.modalService.close(text_id)
  }

  testOk(modal_id: string, id: number): void {
    console.log(modal_id, id)
    this.closeModal(modal_id)
  }

  testCancel(modal_id: string, id: number): void {
    console.log(modal_id, id)
    this.closeModal(modal_id)

  }

}
