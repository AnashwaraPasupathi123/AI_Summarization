import { Routes } from '@angular/router';
import { ChatComponent } from './components/chat/chat';
import { HistoryComponent } from './components/history/history';
import { UploadComponent } from './components/upload/upload';
import { LayoutComponent } from './components/layout/layout';

export const routes: Routes = [
  { path: '', component: LayoutComponent },
  { path: 'history', component: HistoryComponent },
  { path: 'upload', component: UploadComponent }
];
