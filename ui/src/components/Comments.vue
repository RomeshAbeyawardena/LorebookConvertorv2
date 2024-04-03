<script setup lang="ts">
    //import { Comment } from "./models/comment";
    import Card from "primevue/card";
    import { computed, onBeforeMount, onUnmounted } from "vue";
    import { useCommentStore } from "../stores/commentStore";
    import { storeToRefs } from "pinia";
    
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

    onBeforeMount(async() => {
        await commentStore.getComments();
    })
    
    onUnmounted(async() => {
        if(commentStore.hasPendingComments)
        {
            await commentStore.saveComments();
        }
    })

    const props = defineProps({
        entryId:String,
        isReadOnly:Boolean
    });
</script>
<template>
    <div>
        <div>
            <h3>Comments</h3>
        </div>
        <div v-for="comment in entryComments">
            <Card>
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
                                <span>{{ childComment.created }}</span>
                            </div>
                        </div>
                    </div>
                    <div class="flex gap-3 mt-1">
                        <span>{{ comment.created }}</span>
                    </div>
                </template>
            </Card>
        </div>
    </div>
</template>