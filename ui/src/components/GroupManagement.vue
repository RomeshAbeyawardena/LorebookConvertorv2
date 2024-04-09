<script setup lang="ts">
    import { storeToRefs } from 'pinia';
    import { useEntryGroupingStore } from '../stores/EntryGroupingStore';
    import Button from "primevue/button";
    import InputText from 'primevue/inputtext';
    
    import InputGroup from 'primevue/inputgroup';
    import InputGroupAddon from 'primevue/inputgroupaddon';

    import { ref, onBeforeMount, watch } from 'vue';
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
    
        //TODO: Set an interval to check for changes to groups and commit to backend

    onBeforeMount(async() => {
        if(!isGroupsLoaded.value && selectedStory.value) {
            await entryGroupingStore.getGroups(selectedStory.value?.id)
        }

        currentGroups.value = groups.value.filter(g => g.entryIds.some(e => e == props.entryId));
    });
    
    watch(() => props.entryId, (entryId) => {
        currentGroups.value = groups.value.filter(g => g.entryIds.some(e => e == entryId));
    })

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

    function saveGroups() {
        for(let group of groups.value) 
        {
            const index = group.entryIds.indexOf(props.entryId)
            if(index != -1) {
                group.entryIds.splice(index, 1);
            }

            if(currentGroups.value.includes(group)) {
                group.entryIds.push(props.entryId);
            }
        }
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
        <Button label="Save groups" @click="saveGroups" icon="pi pi-disk"></Button>
    </div>
</template>