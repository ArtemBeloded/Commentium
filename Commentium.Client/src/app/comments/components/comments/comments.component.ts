import { Component, OnInit, ViewChild } from "@angular/core";
import { CommentsService } from "../../services/comments.service";
import { PagedList } from "../../types/pagedList.interface";
import { CommentInterface } from "../../types/comment.interface";
import { CommonModule } from "@angular/common";
import { CommentComponent } from "../comment/comment.component";
import { CommentFormComponent } from "../commentForm/commentForm.component";
import { LightboxModule } from "ngx-lightbox";

@Component({
    selector: 'comments',
    standalone: true,
    imports: [CommonModule, CommentComponent, CommentFormComponent, LightboxModule],
    templateUrl: './comments.component.html'
})
export class CommentsComponent implements OnInit {
    
    pagedList: PagedList<CommentInterface> | null = null;
    activeComment: CommentInterface | null = null;
    @ViewChild(CommentFormComponent) commentFormComponent!: CommentFormComponent;

    constructor(private commentsService: CommentsService){}

    ngOnInit(): void {
        this.getPagedListOfComments();
    }

    getPagedListOfComments(): void {
        this.commentsService.getComments().subscribe((pagedList) => {
            this.pagedList = pagedList;
            console.log('getPagedListOfComments')
        });
    }

    addComment({userName, email, commentContent, verifiedFile, parentId }: 
        { userName: string; email: string; commentContent: string; verifiedFile: File | null; parentId: string | null; }): void {
        this.commentsService.createComment(userName, email, commentContent, verifiedFile, parentId).subscribe((response) => {
            this.getPagedListOfComments();
            this.commentFormComponent.resetFormHandler();
        });
        this.activeComment = null;
    }

    setActiveComment(activeComment: CommentInterface | null): void {
        this.activeComment = activeComment;
    }

    sortComments(order: string) : void {
        console.log('sortComments');
    }
}