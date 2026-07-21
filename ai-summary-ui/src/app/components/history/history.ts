import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-history',
  imports: [CommonModule],
  templateUrl: './history.html',
  styleUrl: './history.scss',
})
export class HistoryComponent {
  history: any[] = [];
  ngOnInit() {
    this.history = JSON.parse(localStorage.getItem('history') || '[]');
  }
}
