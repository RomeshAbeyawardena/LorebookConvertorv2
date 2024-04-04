<script setup lang="ts">
    import Card from "primevue/card";
    import Button from "primevue/button";
    import { computed, onBeforeMount } from "vue";
    import { useStoryStore } from "../stores/storyStore";
    import { useCommentStore } from "../stores/commentStore";
    import { storeToRefs } from "pinia";
    import AddComment from "./AddComment.vue";
    import { DateService } from "../services/DateService";
    import { IComment } from "../models/comment";

    const dateService = new DateService();
    
    const commentStore = useCommentStore();
    const { comments, selectedComment } = storeToRefs(commentStore);
    const childComments = computed(() => {
        return (parentMessageId:string) => {
            return comments.value
                .filter(c => c.parentMessageId == parentMessageId);
        }
    });
    const entryComments = computed(() => {
        if(!props.entryId)
        {
            return comments.value.filter(c => c.parentMessageId == undefined);
        }

        return comments.value.filter(c => c.parentMessageId == undefined 
            && c.entryId == props.entryId);
    });

    const storyStore = useStoryStore();

    onBeforeMount(async() => {
        if(storyStore.selectedStory)
        {
            await commentStore.getComments(storyStore.selectedStory.id);
        }
    });

    function editHandler(comment:IComment) {
        if(selectedComment.value 
            && selectedComment.value.messageId == comment.messageId)
        {
            selectedComment.value = undefined;
            return;
        }

        selectedComment.value = comment;
    }

    function setIconClass(messageId:string) {
        
        return selectedComment.value 
            && selectedComment.value.messageId == messageId
            ? "pi pi-times"
            : "pi pi-pencil";
    }

    const props = defineProps({
        entryId:String,
        isReadOnly:Boolean
    });
</script>
<template>
    <div>
        <div    class="border-round border-solid surface-border mt-2 mb-4 p-2"
                v-if="entryComments.length < 1">
            <p class="m-0 p-0">No comments</p>
        </div>
        <div v-for="comment in entryComments">
            <Card   class="mt-2 mb-4 border-round border-solid border-primary"
                    style="border-top-style: none !important">
                <template #header>
                    <div class="chat-header border-round border-noround-bottom"></div>
                </template>
                <template #title>
                    {{ comment.title }}
                </template>
                <template #content>
                    <div class="m-0">
                        <p v-html="comment.message"></p>
                    </div>
                </template>
                <template #footer>
                    <div class="">
                        <div v-for="childComment in childComments(comment.messageId)">
                            <p v-html="childComment.message"></p>
                            <div>
                                <span>
                                    {{ dateService.format(childComment.created) }}
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="flex align-content-center gap-3 mt-1">
                        <Button class="flex align-items-center "
                                @click="editHandler(comment)">
                            <i :class="setIconClass(comment.messageId)"></i>
                        </Button>
                        <div class="flex flex-grow-1"></div>
                        <span class="flex align-items-center justify-content-end">
                            {{ dateService.format(comment.created) }}
                        </span>
                    </div>
                </template>
            </Card>
        </div>
        <div>
            <AddComment v-if="props.entryId" :entry-id="props.entryId" />
        </div>
    </div>
</template>