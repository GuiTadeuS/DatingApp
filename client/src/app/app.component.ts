import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { ModalComponent, ModalService } from './components/modal';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, ModalComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  constructor(private modalService: ModalService) { }

  title = 'client';

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
