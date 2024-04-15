<script setup lang="ts">
    import { useEntryStore } from "../stores/entryStore";
    import { useStoryStore } from "../stores/storyStore";
    import { storeToRefs } from "pinia"; 
    import Dropdown from "primevue/dropdown";
    import { onMounted, ref } from "vue";
    import { IStory } from "../models/story";
    import { useSearchStore } from "../stores/searchStore";
    import Button from "primevue/button";
    import Menu from 'primevue/menu';
    import EntryTreeView from "./EntryTreeView.vue";
    const entryStore = useEntryStore();
    const storyStore = useStoryStore();
    const searchStore = useSearchStore();
    const stories = ref<IStory>()
    const { getOrAddStories, selectedStory } = storeToRefs(storyStore);
    const menu = ref();
    async function changeHandler() {
        //reset stores
        entryStore.isLorebookLoaded = false;
        searchStore.isSearchIndexLoaded = false;
        searchStore.isMapped = false;
        await entryStore.getLorebook();
    }

    const toggle = (event:MouseEvent) => {
        menu.value.toggle(event);
    };

    onMounted(async() => {
        stories.value = await getOrAddStories.value;
    });
</script>
<template>
    <div class="grid mt-2 w-full">
        <div class="col w-full">
            <Dropdown   v-model="selectedStory"
                        @change="changeHandler"
                        class="ml-4 w-full"
                        :options="stories?.data" 
                        option-label="title" 
                        placeholder="Select a story" />
        </div>
        <div class="col align-self-center justify-content-center ">
            
                <h3 class="m-0 flex justify-content-end ">
                    <span class="sm:hidden md:flex align-self-center">
                        Lorebook Viewer
                    </span>
                    <Button class="flex border-round border-primary border-solid ml-2"  aria-haspopup="true" aria-controls="overlay_menu" @click="toggle">
                    <i
                        v-tooltip:left="'Lorebook Viewer'"
                        class="pi pi-book"></i>
                    </Button>
                </h3>
                <Menu ref="menu" :popup="true" class="menu">
                    <template #start>
                        <EntryTreeView>

                        </EntryTreeView>
                    </template>
                </Menu>
        </div>
    </div>
</template>