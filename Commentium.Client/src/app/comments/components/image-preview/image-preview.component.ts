import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from "@angular/core";
import { Lightbox, LightboxModule } from "ngx-lightbox";

@Component({
    selector: 'image-preview',
    standalone: true,
    imports: [CommonModule, LightboxModule],
    templateUrl: './image-preview.component.html'
})
export class ImagePreviewComponent implements OnChanges {
    
    @Input() imageFile: File | null = null;
    @Input() deleteButton: boolean = true;

    @Output() resizedFile = new EventEmitter<File>();
    @Output() fileRemoved = new EventEmitter<void>();
    
    thumbnail: string | null = null;
    fullImage: string | null = null;
    images: any[] = [];

    constructor(private lightbox: Lightbox) {}

    ngOnChanges(changes: SimpleChanges): void {
        console.log('ngOnChanges', this.imageFile)
        this.generateImagePreview();
    }

    private generateImagePreview() {
        const img = new Image();
        const reader = new FileReader();
  
        reader.onload = (e) => {
          img.src = e.target?.result as string;
          img.onload = () => {
            this.fullImage =  this.resizeImage(img, 320, 240);
            this.thumbnail = this.resizeImage(img, 160, 120);
  
            this.images = [{ src: this.fullImage, caption: '', thumb: this.thumbnail }];
            console.log('generateImagePreview',this.images[0]), 
            this.emitResizedImage(this.fullImage);
          };
        };
        
        if(this.imageFile != null){
            reader.readAsDataURL(this.imageFile);
        }
      }
  
      private resizeImage(img: HTMLImageElement, width: number, height: number): string {
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');
        if (!ctx) return '';
  
        canvas.width = width;
        canvas.height = height;
  
        const scale = Math.min(width / img.width, height / img.height);
        const x = (width / 2) - (img.width / 2) * scale;
        const y = (height / 2) - (img.height / 2) * scale;
  
        ctx.drawImage(img, x, y, img.width * scale, img.height * scale);
  
        return canvas.toDataURL('image/jpeg');
      }

      private emitResizedImage(dataUrl: string) {
        const resizedFile = this.dataURLtoFile(dataUrl, 'resized-image.jpg');
        this.resizedFile.emit(resizedFile);
      }
    
      private dataURLtoFile(dataUrl: string, filename: string): File {
        const arr = dataUrl.split(','), mime = arr[0].match(/:(.*?);/)![1];
        const bstr = atob(arr[1]);
        let n = bstr.length;
        const u8arr = new Uint8Array(n);
        while (n--) {
          u8arr[n] = bstr.charCodeAt(n);
        }
        return new File([u8arr], filename, { type: mime });
      }

      openLightbox(index: number): void {
        this.lightbox.open(this.images, index);
      }
    
      closeLightbox(): void {
        this.lightbox.close();
      }

      removeImage(): void {
        this.fileRemoved.emit();
        this.thumbnail = null;
        this.fullImage = null;
        this.images = [];
      }
}