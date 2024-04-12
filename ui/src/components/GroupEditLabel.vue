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
            icon: "pi pi-times",
            command: cancel
        }
    ]);
    
    const group = computed(() => {
        return entryGroupingStore.findGroup(props.groupId);
    });

    const isBeingRenamed = computed(() => {
        return renamedGroup.value && props.groupId == renamedGroup.value.groupId;
    });
    function cancel() {
        renamedGroup.value = undefined;
    }
    function saveGroup() {
        const group = renamedGroup.value;
        if(!group)
        {
            return;
        }

        const foundGroup = entryGroupingStore.findGroup(group.groupId);
        
        if(foundGroup)
        {
            foundGroup.name = renamed.value;
            hasPendingChanges.value = true;
        }

        renamedGroup.value = undefined;
    }
    
    const renamed = ref("");

    function toggleRenamedGroupItem() {
        if(renamedGroup.value == group.value){
            renamedGroup.value = undefined;
        }
        else {
            renamedGroup.value = group.value;
            renamed.value = group.value!.name;
        }
    }

    const setBorder = computed(() => {
        return isBeingRenamed ? "border-none" : "";
    });
</script>
<template>
    <InputGroup>
        <InputGroupAddon :class="setBorder" v-if="!isBeingRenamed">
            <Button size="small" icon="pi pi-pencil" @click="toggleRenamedGroupItem" />
        </InputGroupAddon>
        <InputText :disabled="false" 
                    :tabindex="99999"
                    size="small" 
                    v-if="isBeingRenamed && renamedGroup" 
                    v-model="renamed" />
        <span class="flex flex-auto justify-content-center align-self-center align-items-center" v-if="group && !isBeingRenamed">{{ group.name }}</span>
        <InputGroupAddon v-if="isBeingRenamed">
            <SplitButton :disabled="false" size="small" @click="saveGroup" :model="splitButtonOptions">
                <i class="pi pi-save"></i>
            </SplitButton>
        </InputGroupAddon>
    </InputGroup>
</template>