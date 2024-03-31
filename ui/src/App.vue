<script setup lang="ts">
    import EntryList from "./components/EntryList.vue";
    import Search from "./components/Search.vue";
    import { useEntryStore } from "./stores/entryStore";
    import { onBeforeMount } from "vue";
    import Toast from 'primevue/toast';
    const entryStore = useEntryStore();
    
    onBeforeMount(async() => {
      await entryStore.getLorebook();
    });
</script>
<template>
  <Toast />
  <Search />
  <EntryList />
</template>