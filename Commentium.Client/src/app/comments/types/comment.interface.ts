import { CommentFile } from "./commentFile.interface"

export interface CommentInterface{
    commentId: string,
    userName: string,
    userEmail: string,
    text: string,
    attachedFile: CommentFile | null,
    createdDate: string,
    replies: CommentInterface[]
}