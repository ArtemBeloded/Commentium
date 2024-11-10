import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { CommentInterface } from "../../types/comment.interface";
import { CommentFormComponent } from "../commentForm/commentForm.component";
import { CommentsComponent } from "../comments/comments.component";
import { TextFilePreviewComponent } from "../text-file-preview/text-file-preview.component";
import { ImagePreviewComponent } from "../image-preview/image-preview.component";

@Component({
    selector: 'comment',
    standalone: true,
    imports: [CommonModule, CommentFormComponent, CommentsComponent, TextFilePreviewComponent, ImagePreviewComponent],
    templateUrl: './comment.component.html'
})
export class CommentComponent implements OnInit{
    @Input() comment!: CommentInterface;
    @Input() activeComment!:CommentInterface | null;

    @Output() setActiveComment = new EventEmitter<CommentInterface | null>();
    @Output() addComment = new EventEmitter<{ userName: string; email: string; commentContent: string; verifiedFile: File | null; parentId: string | null }>();

    imageFile: File | null = null;
    textFile: File | null = null;

    ngOnInit(): void {
        this.checkCommentFile();
      }
    
      private checkCommentFile(): void {
        if (this.comment.attachedFile) {
          const { fileName, contentType, content } = this.comment.attachedFile;
    
          const file = new File([content], fileName, { type: contentType });
    
          if (contentType.startsWith('image/')) {
            // Сохраняем файл изображения
            this.imageFile = file;
          } else if (contentType.startsWith('text/')) {
            // Сохраняем текстовый файл
            this.textFile = file;
          }
        }
      }

    

    isReplying(): boolean {
        if(!this.activeComment){
            return false;
        }

        return this.activeComment.commentId === this.comment.commentId
    }


}