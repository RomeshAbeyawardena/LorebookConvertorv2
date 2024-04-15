<script setup lang="ts">
    import StorySelector from "./components/StorySelector.vue";
    import EntryDetails from "./components/EntryDetails.vue";
    import EntryList from "./components/EntryList.vue";
    import Search from "./components/Search.vue";
    import { useEntryStore } from "./stores/entryStore";
    import { onBeforeMount, onBeforeUnmount } from "vue";
    import Toast from 'primevue/toast';
    import { storeToRefs } from "pinia";
    import { useStoryStore } from "./stores/storyStore";
    import { useCommentStore } from "./stores/commentStore";
    import { useNotificationStore } from "./stores/notificationStore";
    import { useEntryGroupingStore } from "./stores/EntryGroupingStore";
    import EntryTreeView from "./components/EntryTreeView.vue";
    
    const storyStore = useStoryStore();
    const entryStore = useEntryStore();
    const commentStore = useCommentStore();
    const { hasPendingComments } = storeToRefs(commentStore)
    const notificationStore = useNotificationStore();
    const entryGroupingStore = useEntryGroupingStore();
    const { hasPendingChanges } = storeToRefs(entryGroupingStore);
    
    async function saveGroups() {
        if(hasPendingChanges.value)
        {
            await entryGroupingStore.saveGroups();
            hasPendingChanges.value = false;
            notificationStore.set({
                title: "Groups saved",
                message: "Groups have been saved",
                severity:"success",
                visible: true,
                lifetime: 3000
            });
        }
    }
    
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
    
    async function saveAll() {
      await saveComments();
      await saveGroups();
    }
    const intervalId = setInterval(async() => {
      await saveAll();
    }, 30000);
    
    onBeforeUnmount(async() => {
        await saveAll();
        clearInterval(intervalId);
    })

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
    <div class="sm:col flex-auto md:col-8">
      <Search v-if="isLorebookLoaded"  />
      <EntryDetails :is-stand-alone="true" 
                    :entry="selectedEntry"
                    v-if="selectedEntry != undefined" />
      <EntryList v-if="isLorebookLoaded && selectedEntry == undefined" />
  </div>
</div>
</template>