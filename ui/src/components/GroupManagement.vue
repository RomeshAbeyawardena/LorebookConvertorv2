<script setup lang="ts">
    import { storeToRefs } from 'pinia';
    import { useEntryGroupingStore } from '../stores/EntryGroupingStore';
    import Button from "primevue/button";
    import { ref, computed, onBeforeMount } from 'vue';
    import MultiSelect from 'primevue/multiselect';
    import { IEntryGroup } from '../models/EntryGroup';
    import { useStoryStore } from '../stores/storyStore';

    const storyStore = useStoryStore();
    const { selectedStory } = storeToRefs(storyStore);
    const props = defineProps({
        entryId: { type: String, required:true }
    });

    const currentGroups = ref<Array<IEntryGroup>>([]);
    const entryGroupingStore = useEntryGroupingStore();
    const { isGroupsLoaded, groups } = storeToRefs(entryGroupingStore);
    
    onBeforeMount(async() => {
        if(!isGroupsLoaded.value && selectedStory.value) {
            await entryGroupingStore.getGroups(selectedStory.value?.id)
        }
    });
    
    const entryGroups = computed(() => {
        return groups.value.filter(g => g.entryIds.some(e => e == props.entryId));
    });
</script>
<template>
    <div>
        <h4>Groups</h4>
        <!-- List the groups-->
        <MultiSelect    option-label="name" 
                        v-model="currentGroups" 
                        :options="entryGroups" 
                        placeholder="Group" />
    </div>
    <div>
        <!-- Modify groups-->
        <Button label="Save groups" icon="pi pi-disk"></Button>
    </div>
</template>