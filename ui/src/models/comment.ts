import { v4 } from "uuid";

export interface IComment {
    messageId:string;
    entryId:string;
    title?:string;
    parentMessageId?:string;
    message:string;
    created:Date;
}

export class Comment {
    public static new(entryId:string, message:string, 
        title?:string, parentMessageId?:string): IComment {
        return {
            messageId: v4(),
            entryId: entryId,
            parentMessageId: parentMessageId,
            message: message,
            title: title,
            created: new Date()
        };
    }
}