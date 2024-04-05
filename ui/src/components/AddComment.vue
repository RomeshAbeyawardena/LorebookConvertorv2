<script setup lang="ts">
    import { ref, onUnmounted } from "vue";
    import { useStoryStore } from "../stores/storyStore";
    import { useCommentStore } from "../stores/commentStore";
    import { storeToRefs } from "pinia";
    
    import InputText from 'primevue/inputtext';
    import Editor from 'primevue/editor';
    import Button from "primevue/button";
    import { Comment } from "../models/comment";
    import { useNotificationStore } from "../stores/notificationStore";
    import { computed, watch } from "vue";
    
    const title = ref("");
    const message = ref("");
    const commentStore = useCommentStore();
    const notificationStore = useNotificationStore();
    const { hasPendingComments, selectedComment } = storeToRefs(commentStore);

    watch(selectedComment, (newValue) => {
        if(newValue)
        {
            title.value = newValue.title ?? title.value;
            message.value = newValue.message;
        }
    })

    async function saveComments() {
        if(hasPendingComments.value)
        {
            await commentStore.saveComments();
            hasPendingComments.value = false;
            notificationStore.set({
                title: "Comments saved",
                message: "Comments have been saved",
                severity:"success",
                visible: true,
                lifetime: 1000
            });
        }
    }
    
    const intervalId = setInterval(saveComments, 30000);

    onUnmounted(async() => {
        await saveComments();
        clearInterval(intervalId);
    })

    const props = defineProps({
        commentId:String,
        entryId:{ type: String, required: true },
        parentMessageId:String
    });

    const storyStore = useStoryStore();
    const { selectedStory } = storeToRefs(storyStore);

    const verb = computed(() => {
        return selectedComment.value 
            ? "Edit"
            : "Add new";
    }) 

    function saveCommentHandler() {
        if(props.entryId && selectedStory.value)
        {
            if(selectedComment.value) {
                selectedComment.value.title = title.value;
                selectedComment.value.message = message.value;
                selectedComment.value = undefined;
            }
            else {
                commentStore.comments.push(Comment
                    .new(selectedStory.value.id, props.entryId, message.value, 
                        title.value, props.parentMessageId));
            }
            
            hasPendingComments.value = true;
            title.value = "";
            message.value = "";
        }
    }
</script>
<template>
    <form>
        <h4 class="mt-2">{{ verb }} Comment</h4>
        <div class="field mt-4">
            <label>Title</label>
            <InputText v-model="title" class="w-full" />
        </div>
        <div class="field">
            <label>Message</label>
            <Editor v-model="message" editor-style="height: 120px">

            </Editor>
        </div>
        <div>
            <Button @click="saveCommentHandler">
                {{ verb }}
            </Button>
        </div>
    </form>
</template>