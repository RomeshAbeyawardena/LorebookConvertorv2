import { defineStore } from "pinia";
import { IComment } from "../models/comment";
import { ref, Ref } from "vue";
import cloneDeep from "lodash/cloneDeep";
import { orderBy } from "lodash";
import localForage from "localforage";

export interface ICommentStore {
    comments:Ref<Array<IComment>>;
    isCommentsLoaded:Ref<boolean>;
    getComment(messageId:string):Promise<IComment|undefined|null>;
    getComments(storyId:string): Promise<Array<IComment>>;
    orderComments(): void;
    saveComments():Promise<void>;
    selectedComment:Ref<IComment|undefined>;
    hasPendingComments:Ref<boolean>;
}

export const useCommentStore = defineStore("comment-store", ():ICommentStore => 
{
    const isCommentsLoaded = ref(false);
    const hasPendingComments = ref(false);
    const selectedComment = ref<IComment|undefined>();
    function orderComments() {
        comments.value = orderBy(comments.value, a => a.created, "desc");
    }
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
        return await navigator.locks.request("comments_".concat(storyId), async() => {
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
            orderComments();
            return comments.value;
        });
    }

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
        orderComments,
        saveComments,
        selectedComment
    };
});