<script setup lang="ts">
    import { ref, onUnmounted } from "vue";
    import { useStoryStore } from "../stores/storyStore";
    import { useCommentStore } from "../stores/commentStore";
    import { storeToRefs } from "pinia";
    
    import InputText from 'primevue/inputtext';
    import Editor from 'primevue/editor';
    import Button from "primevue/button";
    import { Comment } from "../models/comment";

    const title = ref("");
    const message = ref("");
    const commentStore = useCommentStore();

    const { hasPendingComments } = storeToRefs(commentStore);

    onUnmounted(async() => {
        if(hasPendingComments.value)
        {
            await commentStore.saveComments();
            hasPendingComments.value = false;
            console.log("Saved");
        }
    })

    const props = defineProps({
        commentId:String,
        entryId:{ type: String, required: true },
        parentMessageId:String
    });

    const storyStore = useStoryStore();
    const { selectedStory } = storeToRefs(storyStore);
    function saveCommentHandler() {
        if(props.entryId && selectedStory.value)
        {
            commentStore.comments.push(Comment
                .new(selectedStory.value.id, props.entryId, message.value, 
                    title.value, props.parentMessageId));
            hasPendingComments.value = true;
            title.value = "";
            message.value = "";
        }
    }
</script>
<template>
    <form>
        <h4 class="mt-2">Add new Comment</h4>
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
                Save
            </Button>
        </div>
    </form>
</template>