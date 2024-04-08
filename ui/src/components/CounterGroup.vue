<script setup lang="ts">
import Fieldset from 'primevue/fieldset';
defineProps({
    badgeValue: String,
    legend: String,
    toggleIcon: String,
});
</script>
<template>
    <Fieldset class="custom-fieldset" :collapsed="true" toggleable>
        <template #legend>
            <div class="flex justify-content-between align-items-center flex-wrap">
                <h3 class="flex mr-2">{{ legend }}</h3>
                <div class="flex flex-grow-1"></div>
                <div class="flex align-items-center">
                    <Badge class="flex align-items-center" :value="badgeValue" />
                </div>
            </div>
        </template>
        <template #togglericon>
            <div class="flex mr-2">
                <i :class="toggleIcon"></i>
            </div>
        </template>
        <slot></slot>
    </Fieldset>
</template>