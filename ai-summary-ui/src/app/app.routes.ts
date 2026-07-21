import { Routes } from '@angular/router';
import { ChatComponent } from './components/chat/chat';
import { HistoryComponent } from './components/history/history';
import { UploadComponent } from './components/upload/upload';

export const routes: Routes = [
  { path: '', component: ChatComponent },
  { path: 'history', component: HistoryComponent },
  { path: 'upload', component: UploadComponent }
];
