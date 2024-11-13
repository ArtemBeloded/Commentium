import { CommonModule } from "@angular/common";
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { CaptchaComponent } from "../captcha/captcha.component";
import { ImagePreviewComponent } from "../image-preview/image-preview.component";
import { TextFilePreviewComponent } from "../text-file-preview/text-file-preview.component";

@Component({
    selector: 'comment-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, CaptchaComponent, ImagePreviewComponent, TextFilePreviewComponent],
    templateUrl: './commentForm.component.html'
})
export class CommentFormComponent implements OnInit {
    @Input() submitLabel!: string;
    @Input() hasCancelButton: boolean = false;

    @Output() handleSubmit = new EventEmitter<{ userName: string; email: string; commentContent: string; verifiedFile: File | null }>();
    @Output() handleCancel = new EventEmitter<void>();
    @ViewChild(CaptchaComponent) captchaComponent!: CaptchaComponent;
    @ViewChild(ImagePreviewComponent) imagePreviewComponent!: ImagePreviewComponent;
    @ViewChild(TextFilePreviewComponent) textFilePreviewComponent!: TextFilePreviewComponent;

    form!: FormGroup;
    userNamePattern: RegExp = /^[a-zA-Z0-9]+$/;
    emailPattern: RegExp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    isCaptchaValid: boolean = false;

    previewContent: string | null = null;
    isPreviewModalOpen: boolean = false;

    @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
    imageFile: File | null = null;
    textFile: File | null = null;
    fileError: string | null = null;
    verifiedFile: File | null = null;

    constructor(private fb: FormBuilder) {}

    ngOnInit(): void {
      this.createForm();
    }

    createForm(): void {
      this.form = this.fb.group({
        userName: ['', [Validators.required, Validators.pattern(this.userNamePattern)]],
        email: ['', [Validators.required, Validators.pattern(this.emailPattern)]],
        commentContent: ['', [Validators.required, this.validateContent]]
      });
    }

    onSubmit(): void {
      const submissionData = {
        userName: this.form.get('userName')?.value,
        email: this.form.get('email')?.value,
        commentContent: this.form.get('commentContent')?.value,
        verifiedFile: this.verifiedFile
      };
      
      this.handleSubmit.emit(submissionData);
      this.captchaComponent.resetCaptcha();
      this.isCaptchaValid = false;
    }

    onCaptchaValidation(isValid: boolean) {
      this.isCaptchaValid = isValid;
    }

    addTag(tag: string): void {
      const textarea = document.querySelector('.comment-form-textarea') as HTMLTextAreaElement;
      const start = textarea.selectionStart;
      const end = textarea.selectionEnd;
      const value = textarea.value;
      textarea.value = value.substring(0, start) + tag + value.substring(end);
      textarea.focus();
      this.form.controls['title'].setValue(textarea.value);
    }

    validateContent(control: AbstractControl) {
      const pattern = /^(<a href='.*' title='.*'>.*<\/a>|<code>.*<\/code>|<i>.*<\/i>|<strong>.*<\/strong>|[^<>]*)*$/;
      const isValid = pattern.test(control.value);
      return isValid ? null : { invalidContent: true };
    }

    onFileSelected(event: Event) {
      this.fileError = null;

      const input = event.target as HTMLInputElement;
      const file = input.files?.[0];
      
      if (!file) return;

      const isImage = file.type.startsWith('image/');
      const isTextFile = file.type === 'text/plain';
      
      if (isImage) {
          this.imageFile = file;
          return;
      }
      
      if (isTextFile) {
          if (file.size > 100 * 1024) {
              this.fileError = 'Text file is too large. Max size is 100 KB.';
              this.clearFileInput();
          } else {
              this.textFile = file;
              this.verifiedFile = file;
          }
          return;
      }
      
      this.fileError = 'Unsupported file format. Only images (JPG, GIF, PNG) and text files (TXT) are allowed.';
      this.clearFileInput();
    }

    private clearFileInput() {
      if (this.fileInput) {
        this.fileInput.nativeElement.value = '';
      }
    }

    showPreview(): void {
      if (this.form.get('commentContent')?.valid) {
        const rawContent = this.form.get('commentContent')?.value;
        this.previewContent = rawContent;
        this.isPreviewModalOpen = true;
      }
    }

    closePreviewModal(): void {
      this.isPreviewModalOpen = false;
    }

    removeFile(): void {
      this.fileError = null;
      this.imageFile = null;
      this.textFile = null;
      this.verifiedFile = null;
      this.clearFileInput();
    }

    handleResizedFile(resizedFile: File): void {
      this.verifiedFile = resizedFile;
    }

    resetFormHandler(): void {
      this.createForm();
      this.imagePreviewComponent.removeImage();
      this.textFilePreviewComponent.removeTextFile();
    }
}