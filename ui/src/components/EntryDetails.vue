<script setup lang="ts">
    import Button from "primevue/button";
    import Card from 'primevue/card';
    import { computed } from "vue";
    import { IEntry } from '../models/entry';
    import { useNotificationStore } from '../stores/notificationStore'
    const props = defineProps({
        entry: { type: Object, required: true }
    });

    const entry = computed(() => props.entry as IEntry);
    const entryText = computed(() => { 
        const startTag = '<p class="entry-content-paragrah">';
        const str =startTag.concat(entry.value.Text
            .split(".")
            .join(".</p>".concat(startTag)));
            return str.substring(0, str.length - startTag.length); 
        });
    const notificationStore = useNotificationStore();
    async function copyEntry() {
        await navigator.clipboard.writeText(entry.value.Text);
        notificationStore.set({
            title:"Entry copied",
            lifetime: 1000,
            message:"Entry copied to clipboard",
            severity:"info",
            visible: true
        });
    }

    async function copyTitle() {
        await navigator.clipboard.writeText(entry.value.DisplayName);
        notificationStore.set({
            title:"Display name copied",
            lifetime: 1000,
            message:"Display name copied to clipboard",
            severity:"info",
            visible: true
        });
    }
    const keys = computed(() => entry.value.Keys.join(", "));
</script>
<template>
    <Card>
        <template #content>
            <input type="hidden" :id="entry.Id" />
            <div v-html="entryText">

            </div>
        </template>
        <template #footer>
            <div class="flex flex-wrap align-items-center justify-content-between gap-3">
            <div class="flex align-items-center gap-2">
                <Button icon="pi pi-copy" @Click="copyEntry"
                        v-tooltip.top="'Copy text'" 
                        severity="secondary" 
                        rounded text></Button>
                <Button icon="pi pi-clipboard" @Click="copyTitle"
                        v-tooltip.top="'Copy display name'"
                        severity="secondary" 
                        rounded text></Button>
            </div>
            <span class="p-text-secondary">{{ keys }}</span>
            </div>
        </template>
    </Card>
</template>
<style lang="scss">
    p.entry-content-paragrah {
        margin-top: 0;
    }
</style>