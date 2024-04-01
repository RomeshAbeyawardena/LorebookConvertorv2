<script setup lang="ts">
    import StorySelector from "./components/StorySelector.vue";
    import EntryDetails from "./components/EntryDetails.vue";
    import EntryList from "./components/EntryList.vue";
    import Search from "./components/Search.vue";
    import { useEntryStore } from "./stores/entryStore";
    import { onBeforeMount } from "vue";
    import Toast from 'primevue/toast';
    import { storeToRefs } from "pinia";
    import { useStoryStore } from "./stores/storyStore";
    const storyStore = useStoryStore();
    const entryStore = useEntryStore();
    const { selectedEntry, isLorebookLoaded } = storeToRefs(entryStore);
    onBeforeMount(async() => {
      await storyStore.getStories();
    });
</script>
<template>
  <Toast />
  
  <Search v-if="isLorebookLoaded"  />
  <StorySelector />
  <EntryDetails :is-stand-alone="true" 
                :entry="selectedEntry"
                v-if="selectedEntry != undefined" />
  <EntryList v-if="isLorebookLoaded" />
</template>