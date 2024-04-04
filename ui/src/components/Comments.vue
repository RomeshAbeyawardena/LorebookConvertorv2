<script setup lang="ts">
    //import { Comment } from "./models/comment";
    import Card from "primevue/card";
    import { computed, onBeforeMount } from "vue";
    import { useStoryStore } from "../stores/storyStore";
    import { useCommentStore } from "../stores/commentStore";
    import { storeToRefs } from "pinia";
    import AddComment from "./AddComment.vue";
    import { DateService } from "../services/DateService";

    const dateService = new DateService();
    
    const commentStore = useCommentStore();
    const { comments } = storeToRefs(commentStore);
    const childComments = computed(() => {
        return (parentMessageId:string) => {
            return comments.value.filter(c => c.parentMessageId == parentMessageId);
        }
    });
    const entryComments = computed(() => {
        if(!props.entryId)
        {
            return comments.value.filter(c => c.parentMessageId == undefined);
        }

        return comments.value.filter(c => c.parentMessageId == undefined && c.entryId == props.entryId);
    });

    const storyStore = useStoryStore();

    onBeforeMount(async() => {
        if(storyStore.selectedStory)
        {
            await commentStore.getComments(storyStore.selectedStory.id);
        }
    });

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
                                <span>{{ dateService.format(childComment.created) }}</span>
                            </div>
                        </div>
                    </div>
                    <div class="flex gap-3 mt-1">
                        <span>{{ dateService.format(comment.created) }}</span>
                    </div>
                </template>
            </Card>
        </div>
        <div>
            <AddComment v-if="props.entryId" :entry-id="props.entryId" />
        </div>
    </div>
</template>