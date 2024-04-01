import { defineStore } from "pinia";
import { ref, Ref } from "vue";
import { Axios } from "axios";
import { ILorebook } from "../models/lorebook";

export interface IEntryStore {    
    isLorebookLoaded:Ref<boolean>;
    getLorebook: () => Promise<ILorebook>;
    lorebook:Ref<ILorebook>
    categoryIndex:Ref<number>;
    entryIndex:Ref<number>;
}

export const useEntryStore = defineStore("entryStore", ():IEntryStore => {
    const isLorebookLoaded = ref(false);
    const categoryIndex = ref(0);
    const entryIndex = ref(0);
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

    return {
        categoryIndex,
        entryIndex,
        isLorebookLoaded,
        getLorebook,
        lorebook,
    };
});