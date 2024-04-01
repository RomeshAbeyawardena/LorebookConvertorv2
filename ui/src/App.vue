<script setup lang="ts">
    import EntryDetails from "./components/EntryDetails.vue";
    import EntryList from "./components/EntryList.vue";
    import Search from "./components/Search.vue";
    import { useEntryStore } from "./stores/entryStore";
    import { onBeforeMount } from "vue";
    import Toast from 'primevue/toast';
    import { storeToRefs } from "pinia";
    
    const entryStore = useEntryStore();
    const { selectedEntry } = storeToRefs(entryStore);
    onBeforeMount(async() => {
      await entryStore.getLorebook();
    });
</script>
<template>
  <Toast />
  <Search />
  <EntryDetails :is-stand-alone="true" 
                :entry="selectedEntry"
                v-if="selectedEntry != undefined" />
  <EntryList />
</template>