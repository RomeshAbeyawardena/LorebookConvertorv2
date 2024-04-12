import { defineStore, storeToRefs } from "pinia";
import { Ref, ref } from "vue";
import { EntryGroup, IEntryGroup } from "../models/EntryGroup";
import localForage from "localforage";
import { useStoryStore } from "./storyStore";
import { cloneDeep } from "lodash";

export interface IEntryGroupingStore {
    addToGroup:(entryId:string, groupId?:string, groupName?:string) => void;
    findGroup:(groupId?:string, groupName?:string) => IEntryGroup|undefined;
    getGroups(storyId:string):Promise<Array<IEntryGroup>>;
    getGroup(groupId:String):Promise<IEntryGroup|null>;
    hasPendingChanges:Ref<boolean>;
    isGroupsLoaded:Ref<boolean>;
    removeFromGroup:(entryId:string, groupId?:string, groupName?:string) => void;
    groups:Ref<Array<IEntryGroup>>;
    saveGroups():Promise<void>;
    renamedGroup:Ref<IEntryGroup|undefined>;
}

export const useEntryGroupingStore = defineStore("entry-grouping-store", ():IEntryGroupingStore => {
   const hasPendingChanges = ref(false);
    const groups = ref<Array<IEntryGroup>>([]);
   const backend:LocalForage = localForage.createInstance({
        name:"comments",
        version:1.0,
        storeName:"entryGroups"
    });
    const renamedGroup = ref<IEntryGroup|undefined>();
    const storyStore = useStoryStore();

    const { isStoriesLoaded, selectedStory } = storeToRefs(storyStore);

    const isGroupsLoaded = ref(false);

    async function getGroup(groupId:string) {
        return await backend.getItem<IEntryGroup>(groupId);
    } 

    async function saveGroups() {
        var mapped : IEntryGroup[] = groups.value.map(c => cloneDeep(c) as IEntryGroup);
        for(let entryGroup of mapped)
        {
            await backend.setItem(entryGroup.groupId, entryGroup);
        }
    }

    async function getGroups(storyId:string) {   
        return await navigator.locks.request("getGroups", async() => {
            if(!isGroupsLoaded.value)
            {
                const keys = await backend.keys();

                for(let key of keys) {
                    const item = await getGroup(key);
                    if(item && item.storyId == storyId)
                    {
                        groups.value.push(item);
                    }
                }
                isGroupsLoaded.value = true;
            }

            return groups.value;
        });
    }

   function findGroup(groupId?:string, groupName?:string) {
    let group:IEntryGroup|undefined;

    if(groupId) {
        group = groups.value.find(g => g.groupId == groupId);
    }

    if(!group && groupName) {
        group = groups.value.find(g => g.name == groupName);
    }

    return group;
   } 
   
   function addToGroup(entryId:string, groupId?:string, groupName?:string) {
        let group = findGroup(groupId, groupName);

        if(!isStoriesLoaded.value || !selectedStory.value) {
            throw new Error("Selected Story not selected");
        }

        if(!groupName) {
            throw new Error("Unable to create a group with a name");
        }

        if( !group) {
            group = EntryGroup.new(selectedStory.value.id, groupName, [entryId]);
        }
        else {
            group.entryIds.push(entryId);
        }

        groups.value.push(group);
    }

    function removeFromGroup(entryId:string, groupId?:string, groupName?:string)
    {
        const group = findGroup(groupId, groupName);

        if(!group) {
            throw new Error("Group not found");
        }

        const index = group.entryIds.findIndex(f => f == entryId);
        if(index != -1) {
            group.entryIds.splice(index, 1);
        }
    }

    return {
        addToGroup,
        isGroupsLoaded,
        findGroup,
        getGroup,
        getGroups,
        renamedGroup,
        removeFromGroup,
        groups,
        saveGroups,
        hasPendingChanges
   }
});