<script setup lang="ts">
    import { ref } from "vue";
    import { useStoryStore } from "../stores/storyStore";
    import { useCommentStore } from "../stores/commentStore";
    import { storeToRefs } from "pinia";
    
    import InputText from 'primevue/inputtext';
    import Editor from 'primevue/editor';
    import Button from "primevue/button";
    import { Comment } from "../models/comment";
    import { computed, watch } from "vue";
    
    const title = ref("");
    const message = ref("");
    const commentStore = useCommentStore();
    const { hasPendingComments, selectedComment } = storeToRefs(commentStore);

    watch(selectedComment, (newValue) => {
        if(newValue)
        {
            title.value = newValue.title ?? title.value;
            message.value = newValue.message;
        }
        else {
            title.value = "";
            message.value = "";
        }
    });

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
    });

    const saveIcon = computed(() => {
        return selectedComment.value
            ? "pi pi-pencil"
            : "pi pi-plus";
    });

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
                commentStore.orderComments();
            }
            
            hasPendingComments.value = true;
            title.value = "";
            message.value = "";
        }
    }

    function cancelEditHandler() {
        selectedComment.value = undefined;
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
            <Button :icon="saveIcon" 
                    :label="verb" 
                    @click="saveCommentHandler">
            </Button>
            <Button icon="pi pi-times"
                    label="Cancel editing"
                    class="ml-2"
                    severity="danger"
                    v-if="selectedComment"
                    @click="cancelEditHandler">

            </Button>
        </div>
    </form>
</template>