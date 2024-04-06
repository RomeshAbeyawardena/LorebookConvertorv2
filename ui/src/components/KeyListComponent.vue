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
    <ul v-for="item in props.entries" class="flex list-none">
        <li class="flex-auto">
            <a href="javascript:void(0)" @click="copyHandler(item)">
                <i class="pi pi-copy ml-2"></i>
            </a>
            <span>{{ item }}</span>
        </li>
    </ul>
</template>