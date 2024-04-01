import { defineStore } from "pinia";
import { ref, Ref } from "vue";
import { Axios } from "axios";
import { ILorebook } from "../models/lorebook";
import { IEntry } from "../models/entry";

export interface IEntryStore {    
    isLorebookLoaded:Ref<boolean>;
    getLorebook: () => Promise<ILorebook>;
    lorebook:Ref<ILorebook>
    categoryIndex:Ref<number|undefined>;
    entryIndex:Ref<number|undefined>;
    selectedEntry:Ref<IEntry|undefined>;
}

export const useEntryStore = defineStore("entryStore", ():IEntryStore => {
    const isLorebookLoaded = ref(false);
    const categoryIndex = ref<number|undefined>(undefined);
    const entryIndex = ref<number|undefined>(undefined);
    const axios:Axios = new Axios({
        baseURL: "/"
    });
    
    const lorebook = ref<ILorebook>({
        Entries: [],
        Categories: [],
        Groupings: []
    });

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

    const selectedEntry = ref<IEntry|undefined>();

    return {
        categoryIndex,
        entryIndex,
        isLorebookLoaded,
        getLorebook,
        lorebook,
        selectedEntry
    };
});