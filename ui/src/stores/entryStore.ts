import { defineStore } from "pinia";
import { computed, ComputedRef, ref, Ref } from "vue";
import { Axios } from "axios";
import { ILorebook } from "../models/lorebook";
import { ISearchIndex } from "../models/searchIndex";
import { ILorebookGroup } from "../models/groups";

export interface IEntryStore {
    filteredCategories:ComputedRef<Array<ILorebookGroup>>;
    getOrAddSearchIndex:ComputedRef<Array<ISearchIndex>>;
    isLorebookLoaded:Ref<boolean>;
    isSearchIndexLoaded:Ref<boolean>;
    generateSearchIndex:() => void;
    getLorebook: () => Promise<ILorebook>;
    lorebook:Ref<ILorebook>
    searchIndex:Ref<Array<ISearchIndex>>;
    searchText:Ref<string>;
    selectedSearchItem:Ref<string>;
}

export const useEntryStore = defineStore("entryStore", ():IEntryStore => {
    const isLorebookLoaded = ref(false);
    
    const axios:Axios = new Axios({
        baseURL: "/"
    });
    
    const lorebook = ref<ILorebook>({
        Entries: [],
        Categories: [],
        Groupings: []
    });

    const isSearchIndexLoaded = ref(false);
    const searchIndex = ref<Array<ISearchIndex>>([]);

    async function getLorebook() : Promise<ILorebook> {
        if(lorebook.value && isLorebookLoaded.value) {
            return lorebook.value;
        }
        
        const respose = await axios.get("journal.json");
        if(respose.data)
        {
            isLorebookLoaded.value = true;
            const result = JSON.parse(respose.data) as ILorebook;
            
            lorebook.value = result;
            return result;
        }

        return {
            Entries: [],
            Categories: [],
            Groupings: []
        };
    }

    function generateSearchIndex() {
        
        if(isLorebookLoaded.value && !isSearchIndexLoaded.value) {
            searchIndex.value.push(... lorebook.value.Categories.map((c) => {
                return {
                    id: c.Id,
                    keys: [c.Name],
                    title: c.Name,
                    summary: c.Name,
                    category: c
                } as ISearchIndex
            }));

            searchIndex.value.push(... lorebook.value.Entries.map(c => {
                return {
                    id: c.CategoryId.concat(",",c.Id),
                    keys: [c.DisplayName,... c.Keys],
                    title: c.DisplayName,
                    summary: c.Text.substring(0, 50),
                    entry: c
                } as ISearchIndex
            }));
            
            isSearchIndexLoaded.value = true;
        }
        return searchIndex.value;
    }

    const getOrAddSearchIndex = computed(() => {
        return generateSearchIndex();
    })

    const searchText = ref("");
    const filteredCategories = computed(() => {
        if(!isLorebookLoaded)
        {
            return [];
        }

        if(searchText.value.length > 3) {
            return lorebook.value.Groupings.filter(f => f.Category.Name
                .toLocaleLowerCase().includes(searchText.value.toLocaleLowerCase()));
        }
        else {
            return lorebook.value.Groupings;
        }
    })

    const selectedSearchItem = ref("");

    return {
        filteredCategories,
        generateSearchIndex,
        getOrAddSearchIndex,
        isSearchIndexLoaded,
        isLorebookLoaded,
        getLorebook,
        lorebook,
        searchIndex,
        searchText,
        selectedSearchItem
    };
});