<script setup lang="ts">
    import Button from "primevue/button";
    import Card from 'primevue/card';
    import Chips from 'primevue/chips';
    import { computed } from "vue";
    import { IEntry } from '../models/entry';
    import { useNotificationStore } from '../stores/notificationStore'
    import { useEntryStore } from "../stores/entryStore";
    import { storeToRefs } from "pinia";
    import { StringService } from "../services/StringService";
    import Comments from "./Comments.vue";
    import CounterGroup from "./CounterGroup.vue";
    import { useCommentStore } from "../stores/commentStore";
    import KeyListComponent from "./KeyListComponent.vue";
    import GroupManagement from "./GroupManagement.vue";
    import { useEntryGroupingStore } from "../stores/EntryGroupingStore";
    const entryStore = useEntryStore();
    const { selectedEntry } = storeToRefs(entryStore);

    const commentStore = useCommentStore();
    const { comments } = storeToRefs(commentStore);
    const entryCommentsLength = computed(() => {
        return comments.value.filter(c => c.entryId == selectedEntry.value?.Id).length;
    });

    const entryGroupingStore = useEntryGroupingStore();
    const { groups } = storeToRefs(entryGroupingStore)
    
    const props = defineProps({
        readOnly: Boolean,
        entry: { type: Object, required: true },
        isStandAlone: { type: Boolean }
    });


    function closeDetailsPanel() 
    {
        selectedEntry.value = undefined;
    }

    function selectEntry() {
        selectedEntry.value = entry.value;
        window.scrollTo(0, 0);
    }

    const entry = computed(() => props.entry as IEntry);
    
    const currentGroups = computed(() => groups.value.filter(g => g.entryIds.some(e => e == entry.value.Id)));

    function createParagraphs(items:Array<string>) {
        const startTag = '<p class="entry-content-paragrah">';
        let str = "";
        items.forEach(s => str = s ? str.concat(startTag, s, ".</p>") : str);
        return str;
    }
    const summaryTextRaw = computed(() => {
        return entry.value.Text.split(".").slice(0, 4).join(". ").concat(".");
    });

    const summaryText = computed(() => {
        return createParagraphs(entry.value.Text.split(".").slice(0, 4));
    });
    
    const entryText = computed(() => { 
        return createParagraphs(entry.value.Text.split("."))
    });

    async function copySummary() {
        await navigator.clipboard.writeText(summaryTextRaw.value);
        notificationStore.set({
            title:"Entry copied",
            lifetime: 1000,
            message:"Entry summary copied to clipboard",
            severity:"info",
            visible: true
        });
    }

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

    const singularCategoryName = computed(() => {
        const categoryName = entry.value.Category.Name;

        if(categoryName.endsWith('s')) {
            return categoryName.substring(0, categoryName.length - 1);
        }

        return categoryName;
    });

    //const keys = computed(() => entry.value.Keys.join(", "));
</script>
<template>
    <Card :class="setCardClass()">
        <template v-if="props.isStandAlone" #header>
            <div class="flex flex-auto align-items-center">
                <Button @click="closeDetailsPanel"
                        class="flex p-2 border-round" 
                        severity="primary"
                        v-tooltip.top="'Go back'">
                    <i class="pi pi-arrow-circle-left"></i>
                </Button>
                <h1 v-tooltip:right="entry.DisplayName" class="flex align-self align-items-center flex-grow-1 m-0 ml-2 justify-content-end text-right">
                    <a href="javascript:void(0)" class="entry-details-title no-underline text-color text-overflow-ellipsis white-space-nowrap overflow-hidden">
                        {{ entry.DisplayName }}
                    </a>
                </h1>
            </div>
        </template>
        <template #content>
                <input type="hidden" :id="entry.Id" />
                <h2 v-if="isStandAlone">Summary</h2>
                <div v-html="props.isStandAlone ? entryText : summaryText"></div>
                <div v-if="props.isStandAlone">
                    <h3>{{ singularCategoryName }} keys</h3>
                    <Chips :modelValue="entry.Keys" class="w-full" :disabled="true" />
                </div>
                <div v-if="!props.isStandAlone">
                    <p class="border-round border-dotted border-primary p-2 m-0 mt-4 font-semibold text-center">
                        To read more click the <i class="pi pi-expand mr-2 ml-2"></i> 'View details' button</p>
                </div>
                <CounterGroup   legend="Comments" toggle-icon="pi pi-comments"
                                :badge-value="entryCommentsLength.toString()"
                                v-if="props.isStandAlone">
                                <Comments :entryId="entry.Id" />
                </CounterGroup>
                <CounterGroup   legend="Groups" toggle-icon="pi pi-th-large"
                                :badge-value="currentGroups.length.toString()">
                    <GroupManagement :entry-id="entry.Id" :read-only="readOnly" />
                </CounterGroup>
        </template>
        <template #footer>
            <div class="flex flex-wrap align-items-center justify-content-between gap-3">
            <div class="flex align-items-center gap-2">
                <Button icon="pi pi-receipt" @click="copySummary"
                        v-if="!props.isStandAlone"
                        v-tooltip.top="'Copy summary'"
                        severity="secondary"></Button>
                <Button icon="pi pi-copy" @click="copyEntry"
                        v-tooltip.top="'Copy text'" 
                        severity="secondary" 
                        rounded text></Button>
                <Button icon="pi pi-clipboard" @click="copyTitle"
                        v-tooltip.top="'Copy display name'"
                        severity="secondary" 
                        rounded text></Button>
                <Button icon="pi pi-expand" @click="selectEntry"
                        v-if="!props.isStandAlone"
                        v-tooltip.top="'View details'"
                        severity="secondary"
                        rounded text></Button>
            </div>
            <span class="p-text-secondary">
                <KeyListComponent :entries="entry.Keys" />
            </span>
            </div>
        </template>
    </Card>
</template>
<style lang="scss">
    h3 {
        margin: 0;
    }
    p.entry-content-paragrah {
        margin-top: 0;
    }
</style>