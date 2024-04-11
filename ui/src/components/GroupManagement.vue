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
    import { useNotificationStore } from '../stores/notificationStore';

    const notificationStore = useNotificationStore();
    const currentGroups = ref<Array<IEntryGroup>>([]);

    const storyStore = useStoryStore();
    const { selectedStory } = storeToRefs(storyStore);
    const props = defineProps({
        readOnly: Boolean,
        entryId: { type: String, required:true }
    });

    const entryGroupingStore = useEntryGroupingStore();
    const { hasPendingChanges, isGroupsLoaded, groups } = storeToRefs(entryGroupingStore);
    
    onBeforeMount(async() => {
        if(!isGroupsLoaded.value && selectedStory.value) {
            await entryGroupingStore.getGroups(selectedStory.value?.id)
        }
        updateCurrentGroups();
    });
    
    watch(() => props.entryId, () => {
        updateCurrentGroups();
    });

    function updateCurrentGroups() {
        currentGroups.value = groups.value.filter(g => g.entryIds.some(e => e == props.entryId));
    }

    const groupName = ref("");
    function addGroup() {
        if(selectedStory.value?.id)
        {
            const group = groupName.value.trim()
            if(group.length < 1)
            {
                notificationStore.set({
                    title: "Group must have a name",
                    message: "Group must have a name",
                    severity:"error",
                    visible: true,
                    lifetime: 2500
                });    

                return;
            }

            let entryGroup = groups.value.find(f => f.name == group);
            if(entryGroup)
            {
                if(!entryGroup.entryIds.includes(props.entryId)) {
                    entryGroup.entryIds.push(props.entryId);
                }
            }
            else 
            {
                entryGroup = EntryGroup.new(selectedStory.value?.id, group, [props.entryId]);
                groups.value.push(entryGroup);
            }

            if(!currentGroups.value.includes(entryGroup)) {
                currentGroups.value.push(entryGroup);
            }
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
        hasPendingChanges.value = true;
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
                        :disabled="readOnly"
                        placeholder="Group">
            <template #empty>
                No groups found.
            </template>
            <template #footer>
                <div class="ml-2 mr-2 mb-2">
                    <p class="m-0 p-0 mb-2 mt-2 pl-2">Add a group</p>
                    <InputGroup v-if="!readOnly">
                        <InputText v-model="groupName" class="w-full" placeholder="Group name"></InputText>
                        <InputGroupAddon>
                            <Button icon="pi pi-plus" :disabled="groupName.length < 1" @click="addGroup" />
                        </InputGroupAddon>
                    </InputGroup>
                </div>
            </template>
        </MultiSelect>
    </div>
    <div>
        <!-- Modify groups-->
        <Button label="Save groups" v-if="!readOnly" @click="saveGroups" icon="pi pi-disk"></Button>
    </div>
</template>