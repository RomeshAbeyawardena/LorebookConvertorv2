<script setup lang="ts">
    import { computed, ref } from "vue";
    import Button from "primevue/button";
    import InputText from "primevue/inputtext";
    import InputGroup from 'primevue/inputgroup';
    import InputGroupAddon from 'primevue/inputgroupaddon';
    import { useEntryGroupingStore } from "../stores/EntryGroupingStore";
    import SplitButton from 'primevue/splitbutton';
import { storeToRefs } from "pinia";
    const entryGroupingStore = useEntryGroupingStore();
    
    const { hasPendingChanges, renamedGroup } = storeToRefs(entryGroupingStore);

    const props = defineProps({
        groupId: { type: String, required: true }
    });

    const splitButtonOptions = ref([
        {
            label:"Cancel",
            icon: "pi pi-times"
        }
    ]);
    
    const group = computed(() => {
        return entryGroupingStore.findGroup(props.groupId);
    });

    const isBeingRenamed = computed(() => {
        return renamedGroup.value && props.groupId == renamedGroup.value.groupId;
    });

    function saveGroup() {
        const group = renamedGroup.value;
        if(!group)
        {
            return;
        }

        const foundGroup = entryGroupingStore.findGroup(group.groupId);
        
        if(foundGroup)
        {
            foundGroup.name = group.name;
            hasPendingChanges.value = true;
        }

        renamedGroup.value = undefined;
    }

    function toggleRenamedGroupItem() {
        if(renamedGroup.value == group.value){
            renamedGroup.value = undefined;
        }
        else
            renamedGroup.value = group.value;
    }

</script>
<template>
    <InputGroup>
        <InputGroupAddon v-if="!isBeingRenamed">
            <Button size="small" icon="pi pi-pencil" @click="toggleRenamedGroupItem" />
        </InputGroupAddon>
        <InputText size="small" v-if="renamedGroup" v-model="renamedGroup.name" />
        <span class="flex flex-auto justify-content-center align-self-center align-items-center" v-if="group && !isBeingRenamed">{{ group.name }}</span>
        <InputGroupAddon v-if="isBeingRenamed">
            <SplitButton size="small" @click="saveGroup" :model="splitButtonOptions">
                <i class="pi pi-save"></i>
            </SplitButton>
        </InputGroupAddon>
    </InputGroup>
</template>