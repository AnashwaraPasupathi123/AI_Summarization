import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient} from '@angular/common/http';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-upload',
  imports: [CommonModule],
  templateUrl: './upload.html',
  styleUrl: './upload.scss',
})
export class UploadComponent {
  selectedFile: File | null = null;
  pdfUrl: SafeResourceUrl | null = null;
  constructor(private http: HttpClient, private sanitizer: DomSanitizer) {}
  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
    if (!this.selectedFile) return;
    const fileType = this.selectedFile.type;
    if (fileType === 'application/pdf') {
      const url = URL.createObjectURL(this.selectedFile);
      this.pdfUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    } else {
      this.pdfUrl = null; // Word/Text cannot be previewed
    }
  }
  upload(){
    if (!this.selectedFile) return;
    const formData = new FormData();
    formData.append('file', this.selectedFile);
    fetch('https://sturdy-space-funicular-55j4v59v6pfvpqw-5253.app.github.dev/api/document/upload', {
    method: 'POST',
    body: formData,
    })
    .then(res => res.json())
    .then(res => {
        alert('Document uploaded and processed. Document ID: ' + res.documentId);
        localStorage.setItem('documentId', res.documentId.toString());
    })
    .catch(err => alert('Upload failed'));
  }
}
