import { defineStore } from "pinia";
import { computed, ComputedRef, Ref, ref} from "vue";
import { Axios } from "axios";
import { IStory } from "../models/story";
import { IStoryEntry } from "../models/storyEntry";

export interface IStoryStore {
    isStoriesLoaded:Ref<boolean>;
    getOrAddStories:ComputedRef<Promise<IStory>>;
    getStories():Promise<IStory>;
    selectedStory:Ref<IStoryEntry|undefined>;
    stories:Ref<IStory|undefined>;
}

export const useStoryStore = defineStore("story-store", ():IStoryStore => {
    
    const axios = new Axios({
        baseURL:"/"
    });

    const isStoriesLoaded = ref(false);
    const stories = ref<IStory|undefined>();
    const selectedStory = ref<IStoryEntry|undefined>(undefined);
    async function getStories() {
        if(isStoriesLoaded.value && stories.value) {
            return stories.value;
        }

        const response = await axios.get("stories.json");
        if (response.data) {
            isStoriesLoaded.value = true;
            stories.value = JSON.parse(response.data) as IStory;
            return stories.value;
        }

        return {
            data: []
        }
    }

    const getOrAddStories = computed(async() => {
        return await getStories();
    });
    return {
        isStoriesLoaded,
        getOrAddStories,
        getStories,
        selectedStory,
        stories
    };
});