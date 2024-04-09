import { defineStore } from "pinia";
import { ref, Ref } from "vue";
import { Axios } from "axios";
import { ILorebook } from "../models/lorebook";
import { IEntry } from "../models/entry";
import { useStoryStore } from "./storyStore";

export interface IEntryStore {    
    isLorebookLoaded:Ref<boolean>;
    getLorebook: () => Promise<ILorebook>;
    getEntries:(entryId:Array<string>) => Array<IEntry>;
    lorebook:Ref<ILorebook>
    categoryIndex:Ref<number|undefined>;
    entryIndex:Ref<number|undefined>;
    selectedEntry:Ref<IEntry|undefined>;
}

export const useEntryStore = defineStore("entry-store", ():IEntryStore => {
    const storyStore = useStoryStore();
    
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
        return await navigator.locks.request("getLoreBook", async() => {
            if(lorebook.value && isLorebookLoaded.value) {
                return lorebook.value;
            }
            
            if(
                storyStore.isStoriesLoaded && !storyStore.selectedStory){
                return {
                    Entries: [],
                    Categories: [],
                    Groupings: []
                }
            }
    
            const respose = await axios.get(storyStore.selectedStory?.filePath!);
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
        });
    }

    const selectedEntry = ref<IEntry|undefined>();

    function getEntries(entryIds:Array<string>) {
        if(!isLorebookLoaded) {
            return [];
        }

        return lorebook.value.Entries.filter(e => entryIds.some(i => i == e.Id));
    }

    return {
        categoryIndex,
        entryIndex,
        isLorebookLoaded,
        getEntries,
        getLorebook,
        lorebook,
        selectedEntry
    };
});