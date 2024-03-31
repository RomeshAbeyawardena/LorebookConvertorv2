import { defineStore } from "pinia";
import { useEntryStore } from "./entryStore";
import { ref, Ref } from "vue";
export interface ISearchStore {
    mappedIndexes:Ref<Array<{id:string; categoryIndex:number, entryIndex:number}>>;
    mapIndexes():{id:string; categoryIndex:number, entryIndex:number}[];
}


export const createSearchStore = defineStore("search-store", (): ISearchStore => {
    const entryStore = useEntryStore();
    
    const mappedIndexes = ref<{id:string; categoryIndex:number, entryIndex:number}[]>([]);

    function mapIndexes()
    {
        const mapped = entryStore.lorebook.Groupings.flatMap((g, categoryIndex) => {
            return g.Entries.map((e, entryIndex)=> {
                return {
                    categoryIndex: categoryIndex,
                    entryIndex:entryIndex,
                    id: e.Id
                }
            });
        })

        mappedIndexes.value.push(... mapped);
        
        return mappedIndexes.value;
    }

    return {
        mapIndexes,
        mappedIndexes
    };
});