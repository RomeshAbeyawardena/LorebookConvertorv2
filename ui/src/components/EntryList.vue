<script setup lang="ts">
    import Accordion from 'primevue/accordion';
    import AccordionTab from 'primevue/accordiontab';
    import Badge from 'primevue/badge';
    import Button from 'primevue/button';
    import ButtonGroup from 'primevue/buttongroup';
    import Panel from 'primevue/panel';
    import EntryDetails from "./EntryDetails.vue";
    import InputText from 'primevue/inputtext';
    import { useEntryStore } from "../stores/entryStore";
    import { storeToRefs } from "pinia";
    import { onBeforeMount, ref, watch } from "vue";
    import { useSearchStore } from "../stores/searchStore";
    import { ILorebookGroup } from '../models/groups';
    import { IEntryGroup } from '../models/EntryGroup';
    import { useEntryGroupingStore } from '../stores/EntryGroupingStore';
    
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

    const groupToBeRenamed = ref<IEntryGroup|undefined>();
    const entryGroupingStore = useEntryGroupingStore();
    
    function isCurrentGroupBeingRenamed(groupId:string) {
        return groupToBeRenamed.value && groupToBeRenamed.value.groupId == groupId;
    };
    
    function setIcon(groupId:string) {
        return isCurrentGroupBeingRenamed(groupId) 
            ? "pi pi-times"
            : "pi pi-pencil";
    }

    function setButtonSeverity(groupId:string) {
        return isCurrentGroupBeingRenamed(groupId) 
            ? "danger"
            : "primary";
    }

    function toggleRename(groupId?:string) {
        if(!groupId) {
            return;
        }

        if(isCurrentGroupBeingRenamed(groupId)) {
            groupToBeRenamed.value = undefined;
        }
        else
            groupToBeRenamed.value = entryGroupingStore.findGroup(groupId);
    }

</script>
<template>
    <Accordion  class="mt-2" v-if="isLorebookLoaded" 
                v-model:activeIndex="categoryIndex">
        <AccordionTab  v-for="group in categories">
            <template #header>
                <p>
                    <span v-if="!isCurrentGroupBeingRenamed(group.groupId)">{{  group.Category?.Name }}</span>
                    <InputText v-if="isCurrentGroupBeingRenamed(group.groupId) && groupToBeRenamed" v-model="groupToBeRenamed.name"></InputText>
                </p>
                <ButtonGroup>
                    <Button v-if="isCurrentGroupBeingRenamed(group.groupId) && group.groupId"
                            icon="pi pi-save">
                    </Button>           
                    <Button @click="toggleRename(group.groupId)" 
                            v-if="group.groupId" 
                            :severity="setButtonSeverity(group.groupId)"
                            :icon="setIcon(group.groupId)"></Button>
                    <Button severity="secondary" disabled v-if="!group.groupId"></Button>
                </ButtonGroup>
            </template>
            <template #headericon>
                <Badge :severity="setSeverity(group.groupId != undefined)" :value="group.Entries.length">
                </Badge>
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