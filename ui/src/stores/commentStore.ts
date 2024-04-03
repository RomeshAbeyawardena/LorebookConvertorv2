import { defineStore } from "pinia";
import { IComment } from "../models/comment";
import { ref, Ref } from "vue";
import { IBackend, Backend } from "../services/Backend";

export interface ICommentStore {
    comments:Ref<Array<IComment>>;
    getComments(): Promise<Array<IComment>>;
    saveComments():Promise<void>;
}

export const useCommentStore = defineStore("comment-store", ():ICommentStore => 
{
    const comments = ref<Array<IComment>>([]);
    const backend:IBackend = new Backend();
    const parameters:IDBObjectStoreParameters = {
        keyPath: "messageId",
        autoIncrement:false
    };
    
    backend.configure([
        ["comment", parameters , p => {
            p.createIndex("ID", "messageId", { unique: true });
            p.createIndex("entryId", "entryId");
            p.createIndex("parentMessageId", "parentMessageId");
            p.createIndex("message", "message");
            p.createIndex("created", "created", { multiEntry: true });
        }]
    ]);


    async function getComments() : Promise<Array<IComment>> {
        await backend.open("comments", 1);
        const store = backend.store("comment", "readwrite");
        if(store)
        {
            return await backend.get(store);
        }

        await backend.close();
        return [];
    };

    async function saveComments() {
        await backend.open("comments", 1);
        const store = backend.store("comment", "readwrite");
        if(store)
        {
            await backend.put(store, comments.value, c => c.entryId);
        }

        await backend.close();
    }

    return {
        comments,
        getComments,
        saveComments
    };
});