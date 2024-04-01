<script setup lang="ts">
    import { useEntryStore } from "../stores/entryStore";
    import { useStoryStore } from "../stores/storyStore";
    import { storeToRefs } from "pinia"; 
    import Dropdown, { DropdownChangeEvent } from "primevue/dropdown";
    import { onMounted, ref } from "vue";
    import { IStory } from "../models/story";
    import { useSearchStore } from "../stores/searchStore";
    
    const entryStore = useEntryStore();
    const storyStore = useStoryStore();
    const searchStore = useSearchStore();
    const stories = ref<IStory>()
    const { getOrAddStories, selectedStory } = storeToRefs(storyStore);
    
    async function changeHandler(e:DropdownChangeEvent) {
        console.log(e.value);
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
    <div class="flex-auto mt-2 w-full">
        <div class="flex-grow-1">
            <Dropdown   v-model="selectedStory"
                        @change="changeHandler"
                        :options="stories?.data" 
                        option-label="title" 
                        placeholder="Select a story" />
        </div>
    </div>
</template>