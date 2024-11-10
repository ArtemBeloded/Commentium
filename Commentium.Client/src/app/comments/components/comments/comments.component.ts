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
    sortDirectionNewest: boolean = false;
    sortDirectionUsername: boolean = true;
    sortDirectionEmail: boolean = true;

    @ViewChild(CommentFormComponent) commentFormComponent!: CommentFormComponent;

    constructor(private commentsService: CommentsService){}

    ngOnInit(): void {
        this.getPagedListOfComments();
    }

    getPagedListOfComments(sortBy: string = '', direction: string = 'desc', page: number = 1, pageSize = 25): void {
        this.commentsService.getComments(sortBy, direction, page, pageSize).subscribe((pagedList) => {
            this.pagedList = pagedList;
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

    sortComments(type: string): void {
        let sortBy = '';
        let direction = 'desc';
    
        if (type === 'newest') {
          sortBy = '';
          this.sortDirectionNewest = !this.sortDirectionNewest;
          direction = this.sortDirectionNewest ? 'asc' : 'desc';
        } else if (type === 'username') {
          sortBy = 'username';
          this.sortDirectionUsername = !this.sortDirectionUsername;
          direction = this.sortDirectionUsername ? 'asc' : 'desc';
        } else if (type === 'email') {
          sortBy = 'email';
          this.sortDirectionEmail = !this.sortDirectionEmail;
          direction = this.sortDirectionEmail ? 'asc' : 'desc';
        }
    
        this.getPagedListOfComments(sortBy, direction);
    }

    goToPage(page: number): void {
        if(this.pagedList){
            if (page < 1 || page > Math.ceil(this.pagedList!.totalCount / this.pagedList!.pageSize)) {
                return;
            }
            this.getPagedListOfComments('', 'desc', page, this.pagedList.pageSize);
        }
    }

    totalPages(): number {
        return Math.ceil(this.pagedList!.totalCount / this.pagedList!.pageSize);
    }
}