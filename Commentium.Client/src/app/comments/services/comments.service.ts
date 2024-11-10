import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PagedList } from "../types/pagedList.interface";
import { CommentInterface } from "../types/comment.interface";
import { environment } from "../../../environment/environment";;


@Injectable({
    providedIn: 'root'
})
export class CommentsService {
    private apiUrl: string;
    
    constructor(private httpClient: HttpClient) {
        this.apiUrl = environment.apiUrl;
    }

    getComments(sortBy: string, direction: string, page: number = 1, pageSize: number = 25) : Observable<PagedList<CommentInterface>> {
        let params = new HttpParams();
        params = params.append('SortColumn', sortBy);
        params = params.append('SortOrder', direction);
        params = params.append('page', page);
        params = params.append('pageSize', pageSize);
        return this.httpClient.get<PagedList<CommentInterface>>(
            this.apiUrl + 'api/comments', {params: params});
    }

    createComment(userName: string, email: string, commentContent: string, verifiedFile: File | null, parentId: string | null): Observable<any> {
        const formData = new FormData();
        formData.append('UserName', userName);
        formData.append('Email', email);
        formData.append('Text', commentContent);
        formData.append('ParentCommentId', parentId || '');
        
        if (verifiedFile) {
            formData.append('File', verifiedFile, verifiedFile.name);
        }
        
        return this.httpClient.post(this.apiUrl + 'api/comments/addcomment', formData);
    }
}