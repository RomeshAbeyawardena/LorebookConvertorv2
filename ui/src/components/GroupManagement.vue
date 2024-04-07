<script setup lang="ts">
    import { storeToRefs } from 'pinia';
    import { useEntryGroupingStore } from '../stores/EntryGroupingStore';
    import { Button} from "primevue/button";
    import { ref, computed } from 'vue';
    import MultiSelect from 'primevue/multiselect';
import { IEntryGroup } from '../models/EntryGroup';

    const props = defineProps({
        entryId: { type: String, required:true }
    });
    const currentGroups = ref<Array<IEntryGroup>>([]);
    const entryGroupingStore = useEntryGroupingStore();
    const { groups } = storeToRefs(entryGroupingStore);
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