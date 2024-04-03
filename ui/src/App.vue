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
    import EntryTreeView from "./components/EntryTreeView.vue";
    import Comments from "./components/Comments.vue";
    const storyStore = useStoryStore();
    const entryStore = useEntryStore();
    
    const { selectedEntry, isLorebookLoaded } = storeToRefs(entryStore);
    onBeforeMount(async() => {
      await storyStore.getStories();
    });
</script>
<template>
  <Toast />
  <div class="grid">
    <StorySelector />
    <div class="sm:hidden md:block md-col-4">
      <EntryTreeView />
    </div>
    <div class="col">
      <Search v-if="isLorebookLoaded"  />
      <EntryDetails :is-stand-alone="true" 
                    :entry="selectedEntry"
                    v-if="selectedEntry != undefined" />
      <EntryList v-if="isLorebookLoaded" />
      <Comments></Comments>
  </div>
</div>
</template>