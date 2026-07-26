import { Component } from '@angular/core';
import { ChatComponent } from '../chat/chat';

@Component({
  selector: 'app-layout',
  imports: [ChatComponent],
  templateUrl: './layout.html',
  styleUrl: './layout.scss',
})
export class LayoutComponent {
  newChat()
  {
    localStorage.removeItem('documentId');
    localStorage.removeItem('chatId');
    window.location.reload();
  }
}
