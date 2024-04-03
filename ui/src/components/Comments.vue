<script setup lang="ts">
    //import { Comment } from "./models/comment";
    import Card from "primevue/card";
    import { computed, onBeforeMount } from "vue";
    import { useCommentStore } from "../stores/commentStore";
    import { storeToRefs } from "pinia";
    
    const commentStore = useCommentStore();
    const { comments } = storeToRefs(commentStore);

    const entryComments = computed(() => {
        return comments.value.filter(c => c.parentMessageId == undefined && c.entryId == props.entryId);
    });

    onBeforeMount(async() => {
        await commentStore.getComments();
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
                    <div class="m-0" v-html="comment.message"></div>
                </template>
                <template #footer>
                    <div class="flex gap-3 mt-1">
                        <span>{{ comment.created }}</span>
                    </div>
                </template>
            </Card>
        </div>
    </div>
</template>