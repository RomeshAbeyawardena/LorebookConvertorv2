<script setup lang="ts">
    import Accordion from 'primevue/accordion';
    import AccordionTab from 'primevue/accordiontab';
    import Badge from 'primevue/badge';
    import Panel from 'primevue/panel';
    import EntryDetails from "./EntryDetails.vue";
    import { useEntryStore } from "../stores/entryStore";
    import { storeToRefs } from "pinia";
    import { onBeforeMount, ref, watch } from "vue";
    import { useSearchStore } from "../stores/searchStore";
import { ILorebookGroup } from '../models/groups';
    const searchStore = useSearchStore();
    const entryStore = useEntryStore();
    const { isLorebookLoaded, entryIndex, categoryIndex, selectedEntry
    } = storeToRefs(entryStore);

    const categories = ref<Array<ILorebookGroup>>([]);
    const {
        filteredCategories, selectedSearchItem, searchText
    } = storeToRefs(searchStore);

    onBeforeMount(async() => {
        categories.value = await filteredCategories.value
    })

    function isCollapsed(id:number) {
        return entryIndex.value != id;
    }

    function toggle(index:number) {
        if(entryIndex.value == index){
            entryIndex.value = undefined;
            return;
        }

        entryIndex.value = index;
    }

    watch(selectedSearchItem, (value) => {
        const ids = value.includes(",") 
            ? value.split(",") : [value];
        
        const first = ids[0];
        const indexEntry = searchStore.getOrMapIndexes.find(f => f.id == first);
        
        if(indexEntry) {
            searchText.value = "";
            categoryIndex.value = indexEntry.categoryIndex;
            entryIndex.value  = indexEntry.entryIndex;
            
            if(first) {
                selectedEntry.value = entryStore.lorebook.Entries
                    .find(f => f.Id == first);
            }

            const el = document.getElementById(indexEntry.id);
            
            if(el)
            {  
                if(!selectedEntry.value)
                {
                    window.setTimeout(() => {
                        const parent = el.parentElement;
                        const boundingRectangle = parent?.getBoundingClientRect();
                        
                        window.scrollTo({
                            behavior:"smooth",
                            top:boundingRectangle?.y
                        }),1000
                    });
                }
            }
        }
    });

    function setSeverity(isGroup:boolean) {
        return isGroup ? "info" : "secondary";
    }

</script>
<template>
    <Accordion  class="mt-2" v-if="isLorebookLoaded" 
                v-model:activeIndex="categoryIndex">
        <AccordionTab   :header="group.Category?.Name"
                        v-for="group in categories">
            <template #header>
                <Badge :severity="setSeverity(group.isGroup)" :value="group.Entries.length"></Badge>
            </template>
            <input type="hidden" :id="group.CategoryId" />
            <Panel  v-for="entry, key in group.Entries" 
                    :data-id="entry.Id" 
                    :collapsed="isCollapsed(key)" toggleable
                    class="mb-2">
                    <template #header>
                        <a  class="block flex-grow-1 no-underline text-color" 
                            @click="toggle(key)" href="javascript:void(0)">
                            {{ entry.DisplayName }}
                        </a>
                    </template>
                    <EntryDetails :entry="entry" />
            </Panel>
        </AccordionTab>
    </Accordion>
</template>