import { defineStore } from "pinia";
import { IComment } from "../models/comment";
import { ref, Ref } from "vue";
import cloneDeep from "lodash/cloneDeep";

import localForage from "localforage";

export interface ICommentStore {
    comments:Ref<Array<IComment>>;
    isCommentsLoaded:Ref<boolean>;
    getComment(messageId:string):Promise<IComment|undefined|null>;
    getComments(storyId:string): Promise<Array<IComment>>;
    saveComments():Promise<void>;
    hasPendingComments:Ref<boolean>;
}

export const useCommentStore = defineStore("comment-store", ():ICommentStore => 
{
    const isCommentsLoaded = ref(false);
    const hasPendingComments = ref(false);
    const comments = ref<Array<IComment>>([]);
    const backend:LocalForage = localForage.createInstance({
        name:"comments",
        version:1.0,
        storeName:"comments"
    });

    async function getComment(messageId:string, 
        useCache?:boolean) {
        if(useCache) {
            return comments.value.find(c => c.messageId == messageId);
        }

        return await backend.getItem<IComment>(messageId);
    }

    async function getComments(storyId:string) : Promise<Array<IComment>> {
        if(isCommentsLoaded.value) {
            return comments.value;
        }
        
        const keys = await backend.keys();
         
        for(let key of keys) {
            const item = await getComment(key);
            if(item && item.storyId == storyId)
            {
                comments.value.push(item);
            }
        }
        
        isCommentsLoaded.value = comments.value.length > 0;

        return comments.value;
    };

    async function saveComments() {
        var mapped: IComment[] = comments.value.map(c => cloneDeep(c) as IComment);
        for(let comment of mapped)
        {
            await backend.setItem(comment.messageId, comment);
        }
    }

    return {
        isCommentsLoaded,
        comments,
        getComment,
        getComments,
        hasPendingComments,
        saveComments
    };
});