<script setup lang="ts">
    import Accordion from 'primevue/accordion';
    import AccordionTab from 'primevue/accordiontab';
    import Panel from 'primevue/panel';
    import EntryDetails from "./EntryDetails.vue";
    import { useEntryStore } from "../stores/entryStore";
    import { storeToRefs } from "pinia";
    import { watch } from "vue";
    import { useSearchStore } from "../stores/searchStore";
    const searchStore = useSearchStore();
    const entryStore = useEntryStore();
    const { isLorebookLoaded, filteredCategories, entryIndex,
        selectedSearchItem, searchText, categoryIndex
    } = storeToRefs(entryStore);

    function isCollapsed(id:number) {
        return entryIndex.value != id;
    }

    watch(selectedSearchItem, (value) => {
        const ids = value.includes(",") 
            ? value.split(",") : [value];
        const indexEntry = searchStore.getOrMapIndexes.find(f => f.id == ids[0]);
        console.log(ids, indexEntry, indexEntry?.categoryIndex);
        if(indexEntry) {
            searchText.value = "";
            categoryIndex.value = indexEntry.categoryIndex;
            entryIndex.value  = indexEntry.entryIndex;
        }
    });
    //const itemRefs = ref([])
    //const type = computed(() => {
      //  return selectedSearchItem.value.includes(",")
        //    ? selectedSearchItem.value.split(",") : selectedSearchItem.value;
    //});
</script>
<template>
    <Accordion v-if="isLorebookLoaded" v-model:activeIndex="categoryIndex">
        <AccordionTab   :header="group.Category?.Name"
                        v-for="group in filteredCategories">
            <input type="hidden" :id="group.CategoryId" />
            <Panel  v-for="entry, key in group.Entries" 
                    :data-id="entry.Id"
                    :header="entry.DisplayName" 
                    :collapsed="isCollapsed(key)" toggleable
                    class="mb-2">
                    <EntryDetails :entry="entry" />
            </Panel>
        </AccordionTab>
    </Accordion>
</template>