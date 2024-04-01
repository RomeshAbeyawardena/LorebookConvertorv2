<script setup lang="ts">
    import Dropdown, { DropdownFilterEvent } from 'primevue/dropdown';
    import { useSearchStore } from "../stores/searchStore";
    import { storeToRefs } from "pinia";
    import  { onBeforeMount, ref } from "vue";
    const searchStore = useSearchStore();
    
    const { selectedSearchItem, searchText, 
        isSearchIndexLoaded, getOrAddSearchIndex } = storeToRefs(searchStore);

    function retryHandler() {
        if(!isSearchIndexLoaded.value) {
            const val = getOrAddSearchIndex.value;
            
            if(val.length > 0) {
                console.log("Has entries", val.length);
                return;
            }

            window.setTimeout(retryHandler, 1000);
            console.log("Retrying...");
        }
        else {
            console.log("Has entries", getOrAddSearchIndex.value.length);
        }
    }

    onBeforeMount(() => {
        retryHandler();
    })
    const filterFields = ref(["title","summary","entry"]);
    function filterEventHandler(filterEvent: DropdownFilterEvent) {
        searchText.value = filterEvent.value;
    }
</script>
<template>
    <div class="w-full">
        <Dropdown   class="w-full"
                    optionLabel="title"
                    optionValue="id"
                    v-model="selectedSearchItem"
                    :loading="!isSearchIndexLoaded"
                    :options="getOrAddSearchIndex" 
                    @filter="filterEventHandler"
                    :filterFields="filterFields"
                    filter />
    </div>
</template>