<script setup lang="ts">
    import Accordion from 'primevue/accordion';
    import AccordionTab from 'primevue/accordiontab';
    import Badge from 'primevue/badge';
    import Button from "primevue/button";
    import Panel from 'primevue/panel';
    import EntryDetails from "./EntryDetails.vue";
    import GroupEditLabel from "./GroupEditLabel.vue";

    import { useEntryStore } from "../stores/entryStore";
    import { storeToRefs } from "pinia";
    import { onBeforeMount, watch } from "vue";
    import { useSearchStore } from "../stores/searchStore";
    import { useEntryGroupingStore } from '../stores/EntryGroupingStore';
    import { useStoryStore } from '../stores/storyStore';
    
    const searchStore = useSearchStore();
    const entryStore = useEntryStore();
    const entryGroupingStore = useEntryGroupingStore();

    const storyStore = useStoryStore();
    const { selectedStory } = storeToRefs(storyStore);
    
    const { isLorebookLoaded, entryIndex, categoryIndex, selectedEntry, lorebook
    } = storeToRefs(entryStore);

    
    const {
        filteredCategories, selectedSearchItem, searchText
    } = storeToRefs(searchStore);

    onBeforeMount(async() => {
        if(selectedStory.value)
        {
            await entryGroupingStore.getGroups(selectedStory.value.id);
        }
    });

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

    function viewEntry(entryId:string) {
        selectedEntry.value = lorebook.value.Entries.find(f => f.Id == entryId);
    }
 
</script>
<template>
    <Accordion  class="mt-2" v-if="isLorebookLoaded" 
                v-model:activeIndex="categoryIndex">
        <AccordionTab :tabindex="-1"  v-for="group in filteredCategories">
            <template #header>
               <GroupEditLabel v-if="group.groupId" :group-id="group.groupId" />
               <p   class="flex flex-auto justify-content-center align-self-center align-items-center" 
                    v-if="!group.groupId">{{ group.Category.Name }}</p>
                <Button class="ml-2" severity="secondary" icon="pi pi-book" v-if="!group.groupId" />
            </template>
            <template #headericon>
                <Badge class="ml-2" :severity="setSeverity(group.groupId != undefined)" :value="group.Entries.length" />
            </template>
            <input type="hidden" :id="group.CategoryId" />
            <Panel  v-for="entry, key in group.Entries" 
                    :data-id="entry.Id" 
                    :collapsed="isCollapsed(key)" toggleable
                    class="mb-2">
                    <template #icons>
                        <a href="javascript:void(0)" @click="viewEntry(entry.Id)" class="mr-2">
                            <i class="pi pi-expand"></i>
                        </a>
                    </template>
                    <template #header>
                        <a  class="flex flex-grow-1 no-underline text-color" 
                            @click="toggle(key)" href="javascript:void(0)">
                            <div class="entry-list-title white-space-nowrap overflow-hidden text-overflow-ellipsis">{{ entry.DisplayName }}</div>
                        </a>
                    </template>
                    <EntryDetails :entry="entry" :read-only="true" />
            </Panel>
        </AccordionTab>
    </Accordion>
</template>