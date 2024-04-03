import { defineStore } from "pinia";
import { IComment } from "../models/comment";
import { ref, Ref } from "vue";
import { IBackend, Backend } from "../services/Backend";

export interface ICommentStore {
    comments:Ref<Array<IComment>>;
    getComments(): Promise<Array<IComment>>;
    saveComments():Promise<void>;
    hasPendingComments:Ref<boolean>;
}

export const useCommentStore = defineStore("comment-store", ():ICommentStore => 
{
    const hasPendingComments = ref(false);
    const comments = ref<Array<IComment>>([]);
    const backend:IBackend = new Backend();
    const parameters:IDBObjectStoreParameters = {
        //keyPath: "messageId",
        //autoIncrement:false,
    };
    
    backend.configure([
        ["comment", parameters , p => {
            p.createIndex("ID", "messageId", { unique: true, });
            p.createIndex("storyId", "storyId");
            p.createIndex("entryId", "entryId");
            p.createIndex("parentMessageId", "parentMessageId");
            p.createIndex("message", "message");
            p.createIndex("created", "created");
        }]
    ]);


    async function getComments() : Promise<Array<IComment>> {
        await backend.open("comments", 1);
        const store = backend.store("comment", "readwrite");
        if(store)
        {
            comments.value = await backend.get(store);
            return comments.value;
        }

        await backend.close();
        return [];
    };

    async function saveComments() {
        await backend.open("comments", 1);
        const store = backend.store("comment", "readwrite");
        if(store)
        {
            var mapped: IComment[] = comments.value.map(c => {
                return {
                    storyId:c.storyId,
                    messageId:c.messageId,
                    entryId:c.entryId,
                    parentMessageId:c.parentMessageId,
                    message:c.message,
                    created:c.created,
                };
            });

            await backend.put(store, mapped, "messageId");
            store.transaction.commit();
        }

        await backend.close();
    }

    return {
        comments,
        getComments,
        hasPendingComments,
        saveComments
    };
});