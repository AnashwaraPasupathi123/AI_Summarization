import { Component, ChangeDetectorRef } from '@angular/core';
import { QueryService, QueryResponse} from '../../services/query.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chat',
  imports: [CommonModule, FormsModule],
  templateUrl: './chat.html',
  styleUrl: './chat.scss',
})
export class ChatComponent {
  question = '';
  messages: any[] = [];
  isTyping = false;
  constructor(private queryservice: QueryService, private cd: ChangeDetectorRef){}
  ngOnInit() {
    // Start with a fresh chat view. History is kept in localStorage and
    // shown on the History page, but should not be loaded into the active
    // chat session automatically.
    this.messages = [];
  }
  send(){
    if (!this.question.trim()) return;
    const userMsg = {sender: 'user', text: this.question};
    this.messages.push(userMsg);
    this.saveToHistory(userMsg);
    this.isTyping = true;
    this.queryservice.ask(this.question).subscribe({
      next: (res: QueryResponse) => {
    console.log('Received answer from API', res);
    this.isTyping = false;
    const aiMsg = { sender: 'ai', text: res.answer };
    this.messages.push(aiMsg);
    this.saveToHistory(aiMsg);
    // ensure Angular updates the view immediately
    try { this.cd.detectChanges(); } catch {}
    this.scrollToBottom();
  },
  error: (err) => {
    console.error('Error from API', err);
    this.isTyping = false;
    const errMsg = { sender: 'ai', text: 'Error contacting server.' };
    this.messages.push(errMsg);
    this.saveToHistory(errMsg);
    try { this.cd.detectChanges(); } catch {}
    this.scrollToBottom();
  }
    });
  this.question = '';
  }
  saveToHistory(msg: any) {
    const history = JSON.parse(localStorage.getItem('history') || '[]');
    history.push(msg);
    localStorage.setItem('history', JSON.stringify(history));
  }
  scrollToBottom(){
    setTimeout(() => {
    const el = document.querySelector('.messages');
    if (el) el.scrollTop = el.scrollHeight;
  }, 100);
  }
}
