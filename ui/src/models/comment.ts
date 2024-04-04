import { v4 } from "uuid";

export interface IComment {
    storyId:string;
    messageId:string;
    entryId:string;
    title?:string;
    parentMessageId?:string;
    message:string;
    created:Date;
}

export class Comment {
    public static new(storyId:string, entryId:string, message:string, 
        title?:string, parentMessageId?:string, messageId?:string): IComment {
        return {
            storyId: storyId,
            messageId: messageId ?? v4(),
            entryId: entryId,
            parentMessageId: parentMessageId,
            message: message,
            title: title,
            created: new Date()
        };
    }
}