<script setup lang="ts">
    import { ref, computed, watch } from 'vue';
    import Tree from 'primevue/tree';
    import { TreeNode } from 'primevue/treenode';
    import { useEntryStore } from '../stores/entryStore';
    import { useSearchStore } from '../stores/searchStore';
    import { storeToRefs } from 'pinia';
    const entryStore = useEntryStore();
    const { selectedEntry, isLorebookLoaded } = storeToRefs(entryStore);
    const searchStore = useSearchStore();
    const { filteredCategories } = storeToRefs(searchStore);
    const isSelf = ref(false);
    function nodeSelectHandler(node: TreeNode) {
        isSelf.value = true;
        selectedEntry.value = node.data;
    }

    const nodes = ref<Object>({});
    const expanded = ref<Object>({});
    watch(selectedEntry, (newValue) => {
        
        if(isSelf.value) {
            isSelf.value = false;
            return;
        }

        if(newValue)
        {
            const s = `{"${newValue.Id}": true}`;
            nodes.value = JSON.parse(s);

            const c = `{"${newValue.CategoryId}": true}`;
            expanded.value = JSON.parse(c);
        }
        else 
            nodes.value = {};
    });

    const loreBookNodes = computed(()=> {
        let treeNodes:Array<TreeNode> = [];
        if(isLorebookLoaded) {
            treeNodes = [];
            filteredCategories.value.forEach(i => {
                const categoryEntry:TreeNode = {
                    data: i,
                    icon: "pi pi-folder",
                    key: i.CategoryId,
                    label: i.Category.Name, 
                    type:"category",
                    selectable: false
                }
                
                const entryNodes:Array<TreeNode> = [];
                
                i.Entries.forEach(e => {
                    entryNodes.push({
                        data: e,
                        key: e.Id,
                        label: e.DisplayName,
                        type: "entry",
                        selectable: true
                    });
                });

                categoryEntry.children = entryNodes;
                categoryEntry.leaf = entryNodes.length > 0;
                treeNodes.push(categoryEntry);
            });
        }
        return treeNodes;
    });
</script>
<template>
    <Tree   class="w-full" 
            :value="loreBookNodes" 
            selection-mode="single" 
            @node-select="nodeSelectHandler"
            v-model:expanded-keys="expanded"
            v-model:selection-keys="nodes">
        <template #default="slotprops">
            <p class="m-0 p-0 text-overflow-ellipsis white-space-nowrap" style="width:300px">{{ slotprops.node.label }}</p>
        </template>
    </Tree>
</template>