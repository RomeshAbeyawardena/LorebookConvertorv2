import { defineStore } from "pinia";
import { useEntryStore } from "./entryStore";
import { IMappedIndex } from "../models/mapped-index";
import { computed, ComputedRef, ref, Ref } from "vue";
export interface ISearchStore {
    getOrMapIndexes:ComputedRef<Array<IMappedIndex>>;
    isMapped:Ref<boolean>;
    mappedIndexes:Ref<Array<IMappedIndex>>;
    mapIndexes():Array<IMappedIndex>;
}


export const useSearchStore = defineStore("search-store", (): ISearchStore => {
    const entryStore = useEntryStore();
    const isMapped = ref(false);
    const mappedIndexes = ref<Array<IMappedIndex>>([]);

    function mapIndexes()
    {
        if(isMapped.value) {
            return mappedIndexes.value;
        }
        
        if (!entryStore.isLorebookLoaded)
        {
            return [];
        }

        const mapped = entryStore.lorebook.Groupings.flatMap((g, categoryIndex) => {
            return g.Entries.map((e, entryIndex)=> {
                return {
                    categoryIndex: categoryIndex,
                    entryIndex:entryIndex,
                    id: e.Id
                }
            });
        });
        console.log(mapped);
        mappedIndexes.value.push(... mapped);
        isMapped.value = true;
        return mappedIndexes.value;
    }

    const getOrMapIndexes = computed(() => mapIndexes()); 

    return {
        isMapped,
        getOrMapIndexes,
        mapIndexes,
        mappedIndexes
    };
});