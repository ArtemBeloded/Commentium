import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from "@angular/core";

@Component({
    selector: 'text-file-preview',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './text-file-preview.component.html'
})
export class TextFilePreviewComponent implements OnChanges {

    @Input() deleteButton: boolean = true;
    @Input() textFile: File | null = null;

    @Output() textFileRemoved = new EventEmitter<void>();

    fullText: string | null = null;
    thumbnailText: string | null = null;
    isTextModalOpen: boolean = false;

    ngOnChanges(changes: SimpleChanges): void {
        this.handleTextFile();
    }

    private handleTextFile() {
        if(this.textFile == null){
            return;
        }

        this.thumbnailText = this.createImageFromText(160, 120);
    
        const reader = new FileReader();
        reader.onload = (e) => {
            this.fullText = e.target?.result as string || '';
        };
        reader.readAsText(this.textFile);
    }
    
    private createImageFromText(width: number, height: number): string {
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');
        if (!ctx) return '';
  
        canvas.width = width;
        canvas.height = height;
  
        ctx.fillStyle = '#f0f0f0';
        ctx.fillRect(0, 0, width, height);
        ctx.fillStyle = '#333333';
        ctx.font = 'bold 24px Arial';
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        
        ctx.fillText('.txt', width / 2, height / 2);
  
        return canvas.toDataURL('image/png');
    }

    openTextFileModal(): void {
        this.isTextModalOpen = true;
    }
  
    closeTextFileModal(): void {
        this.isTextModalOpen = false;
    }

    removeTextFile(): void {
        this.textFileRemoved.emit();
        this.fullText = null;
        this.thumbnailText = null;
    }
}