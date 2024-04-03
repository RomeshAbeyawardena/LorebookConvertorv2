import { v4 } from "uuid";

export interface IComment {
    messageId:string;
    entryId:string;
    parentMessageId?:string;
    message:string;
    created:Date;
}

export class Comment {
    public static new(entryId:string, parentMessageId:string, 
            message:string): IComment {
        return {
            messageId: v4(),
            entryId: entryId,
            parentMessageId: parentMessageId,
            message: message,
            created: new Date()
        };
    }
}