import { defineStore } from "pinia";
import { IComment } from "../models/comment";
import { ref, Ref } from "vue";

export interface ICommentStore {
    comments:Ref<Array<IComment>>;
    getComments(): Promise<Array<IComment>>;
    saveComments():Promise<void>;
}

export const useCommentStore = defineStore("comment-store", ():ICommentStore => 
{
    
    const comments = ref<Array<IComment>>([]);
    async function getComments() : Promise<Array<IComment>> {
        return [];
    };

    async function saveComments() {
        const response = indexedDB.open("comments", 1);
        //response.onsuccess()
    }

    return {
        comments,
        getComments,
        saveComments
    };
});