<script setup lang="ts">
    import Accordion from 'primevue/accordion';
    import AccordionTab from 'primevue/accordiontab';
    import Panel from 'primevue/panel';
    import EntryDetails from "./EntryDetails.vue";
    import { useEntryStore } from "../stores/entryStore";
    import { storeToRefs } from "pinia";
    //import { computed } from "vue";
    const entryStore = useEntryStore();
    const { isLorebookLoaded, filteredCategories,
        //selectedSearchItem 
    } = storeToRefs(entryStore);

    //const itemRefs = ref([])
    //const type = computed(() => {
      //  return selectedSearchItem.value.includes(",")
        //    ? selectedSearchItem.value.split(",") : selectedSearchItem.value;
    //});
</script>
<template>
    <Accordion v-if="isLorebookLoaded">
        <AccordionTab   :header="group.Category?.Name"
                        v-for="group in filteredCategories">
            <input type="hidden" :id="group.CategoryId" />
            <Panel  v-for="entry in group.Entries" 
                    :data-id="entry.Id"
                    :header="entry.DisplayName" 
                    :collapsed="true" toggleable
                    class="mb-2">
                    <EntryDetails :entry="entry" />
            </Panel>
        </AccordionTab>
    </Accordion>
</template>