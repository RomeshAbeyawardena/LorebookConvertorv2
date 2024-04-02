<script setup lang="ts">
    import { useEntryStore } from "../stores/entryStore";
    import { useStoryStore } from "../stores/storyStore";
    import { storeToRefs } from "pinia"; 
    import Dropdown from "primevue/dropdown";
    import { onMounted, ref } from "vue";
    import { IStory } from "../models/story";
    import { useSearchStore } from "../stores/searchStore";
    
    const entryStore = useEntryStore();
    const storyStore = useStoryStore();
    const searchStore = useSearchStore();
    const stories = ref<IStory>()
    const { getOrAddStories, selectedStory } = storeToRefs(storyStore);
    
    async function changeHandler() {
        //reset stores
        entryStore.isLorebookLoaded = false;
        searchStore.isSearchIndexLoaded = false;
        searchStore.isMapped = false;
        await entryStore.getLorebook();
    }

    onMounted(async() => {
        stories.value = await getOrAddStories.value;
    });
</script>
<template>
    <div class="grid mt-2 w-full">
        <div class="col w-full">
            <Dropdown   v-model="selectedStory"
                        @change="changeHandler"
                        class="w-full"
                        :options="stories?.data" 
                        option-label="title" 
                        placeholder="Select a story" />
        </div>
        <div class="col align-self-center justify-content-center ">
            <h3 class="m-0 flex justify-content-end ">
                <span class="sm:hidden md:flex align-self-center">
                    Lorebook Viewer
                </span>
                <i  style="color: var(--primary-color)"
                    v-tooltip:top="'Lorebook Viewer'"
                    class="flex border-round border-primary border-solid pi pi-book ml-2 p-2"></i>
            </h3>
        </div>
    </div>
</template>