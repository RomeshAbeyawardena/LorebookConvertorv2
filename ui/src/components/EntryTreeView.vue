<script setup lang="ts">
    import { computed } from 'vue';
    import Tree from 'primevue/tree';
    import { TreeNode } from 'primevue/treenode';
    import { useEntryStore } from '../stores/entryStore';
    import { storeToRefs } from 'pinia';
    const entryStore = useEntryStore();
    const { selectedEntry, isLorebookLoaded, lorebook } = storeToRefs(entryStore);
    function nodeSelectHandler(node: TreeNode) {
        console.log(node);
        selectedEntry.value = node.data;
    }
    const loreBookNodes = computed(()=> {
        let treeNodes:Array<TreeNode> = [];
        if(isLorebookLoaded) {
            treeNodes = [];
            lorebook.value.Groupings.forEach(i => {
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
    <Tree :value="loreBookNodes" selection-mode="single" @node-select="nodeSelectHandler" />
</template>