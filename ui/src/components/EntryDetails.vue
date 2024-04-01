<script setup lang="ts">
    import Button from "primevue/button";
    import Card from 'primevue/card';
    import { computed } from "vue";
    import { IEntry } from '../models/entry';
    import { useNotificationStore } from '../stores/notificationStore'
    import { useEntryStore } from "../stores/entryStore";
    import { storeToRefs } from "pinia";
    import { StringService } from "../services/StringService";

    const entryStore = useEntryStore();
    const { selectedEntry } = storeToRefs(entryStore);

    const props = defineProps({
        entry: { type: Object, required: true },
        isStandAlone: { type: Boolean }
    });

    function CloseDetailsPanel() 
    {
        selectedEntry.value = undefined;
    }

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

    function setCardClass() {
        const defaultValue = "p-2 border-rounded border-solid surface-border";
        if(props.isStandAlone){
            return "mt-3 ".concat(defaultValue);
        }

        return defaultValue;
    }

    async function copyTitle() {
        const s = StringService
            .ArrayToSentence(entry.value.DisplayName, entry.value.Keys);
        await navigator.clipboard.writeText(s.length > 0
            ? entry.value.DisplayName.concat(" also known as ", s)
            : entry.value.DisplayName);

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
    <Card :class="setCardClass()">
        <template v-if="props.isStandAlone" #header>
            <div class="flex flex-auto">
                <Button @click="CloseDetailsPanel"
                        class="flex p-2 border-round" 
                        severity="primary"
                        v-tooltip.top="'Go back'">
                    <i class="pi pi-arrow-circle-left"></i>
                </Button>
                <h1 class="flex flex-grow-1 m-0 ml-2 justify-content-end">
                    {{ entry.DisplayName }}
                </h1>
            </div>
        </template>
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