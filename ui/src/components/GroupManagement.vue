<script setup lang="ts">
    import { storeToRefs } from 'pinia';
    import { useEntryGroupingStore } from '../stores/EntryGroupingStore';
    import { useEntryStore } from '../stores/entryStore';
    import { computed } from 'vue';

    const props = defineProps({
        entryId: { type: String, required:true }
    });

    const entryStore = useEntryStore();

    const entryGroupingStore = useEntryGroupingStore();
    const { groups } = storeToRefs(entryGroupingStore);
    const entryGroups = computed(() => {
        return groups.value.filter(g => g.entryIds.some(e => e == props.entryId)).flatMap(h => entryStore.getEntries(h.entryIds));
    });
</script>
<template>
    <div>
        <h4>Groups</h4>
        <!-- List the groups-->
    </div>
    <div>
        <!-- Modify groups-->
    </div>
</template>