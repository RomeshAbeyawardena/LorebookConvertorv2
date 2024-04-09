<script setup lang="ts">
    import { storeToRefs } from 'pinia';
    import { useEntryGroupingStore } from '../stores/EntryGroupingStore';
    import Button from "primevue/button";
    import InputText from 'primevue/inputtext';
    
    import InputGroup from 'primevue/inputgroup';
    import InputGroupAddon from 'primevue/inputgroupaddon';

    import { ref, onBeforeMount } from 'vue';
    import MultiSelect from 'primevue/multiselect';
    import { EntryGroup, IEntryGroup } from '../models/EntryGroup';
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

        currentGroups.value = groups.value.filter(g => g.entryIds.some(e => e == props.entryId));
    });
    
    const groupName = ref("");
    function addGroup() {
        if(selectedStory.value?.id)
        {
            const entryGroup = EntryGroup.new(selectedStory.value?.id, groupName.value, [props.entryId]);
            groups.value.push(entryGroup);
            currentGroups.value.push(entryGroup);
        }

        groupName.value = "";
    }
</script>
<template>
    <div class="mb-2">
        <h4>Groups</h4>
        <!-- List the groups-->
        <MultiSelect    option-label="name" 
                        class="w-full"
                        v-model="currentGroups" 
                        :options="groups" 
                        placeholder="Group">
            <template #empty>
                No groups found.
            </template>
            <template #footer>
                <div class="ml-2 mr-2 mb-2">
                    <p class="m-0 p-0 mb-2 mt-2 pl-2">Add a group</p>
                    <InputGroup>
                        <InputText v-model="groupName" class="w-full" placeholder="Group name"></InputText>
                        <InputGroupAddon>
                            <Button icon="pi pi-plus" @click="addGroup" />
                        </InputGroupAddon>
                    </InputGroup>
                </div>
            </template>
        </MultiSelect>
    </div>
    <div>
        <!-- Modify groups-->
        <Button label="Save groups" icon="pi pi-disk"></Button>
    </div>
</template>