import { defineStore } from "pinia";
import { IComment } from "../models/comment";
import { ref, Ref } from "vue";
import { Backend } from "../services/Backend";

export interface ICommentStore {
    comments:Ref<Array<IComment>>;
    getComments(): Promise<Array<IComment>>;
    saveComments():Promise<void>;
}

export const useCommentStore = defineStore("comment-store", ():ICommentStore => 
{
    const comments = ref<Array<IComment>>([]);
    async function getComments() : Promise<Array<IComment>> {
        const backend = new Backend();
        
        const parameters:IDBObjectStoreParameters = {
            keyPath: "messageId",
            autoIncrement:false
        }
        
        backend.configure([
            ["comments", parameters , p => {
                p.createIndex("ID", "messageId", { unique: true });
                p.createIndex("entryId", "entryId");
                p.createIndex("parentMessageId", "parentMessageId");
                p.createIndex("message", "message");
                p.createIndex("created", "created", { multiEntry: true });
            }]
        ])
        await backend.open("comments", 1);

        return [];
    };

    async function saveComments() {
        //response.onsuccess()
    }

    return {
        comments,
        getComments,
        saveComments
    };
});