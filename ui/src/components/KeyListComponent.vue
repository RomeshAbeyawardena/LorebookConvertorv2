<script setup lang="ts">
    import { useNotificationStore } from "../stores/notificationStore";
    const props = defineProps({
        entries:Array<string>
    });

    const notificationStore = useNotificationStore();

    async function copyHandler(item:string) {
        await navigator.clipboard.writeText(item);

        notificationStore.set({
            title:"Kety copied",
            lifetime: 1000,
            message:"Key copied to clipboard",
            severity:"info",
            visible: true
        });
    }

</script>
<template>
    <ul class="flex list-none flex-row flex-wrap">
        <li v-for="item in props.entries" class="flex flex-auto">
            <a href="javascript:void(0)" class="ml-2" @click="copyHandler(item)">
                <i class="pi pi-copy"></i>
            </a>
            <span>{{ item }}</span>
        </li>
    </ul>
</template>