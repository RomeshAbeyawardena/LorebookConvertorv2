import { defineStore } from "pinia";
import { Ref, ref } from "vue";
import { EntryGroup, IEntryGroup } from "../models/EntryGroup";
import localForage from "localforage";

export interface IEntryGroupingStore {
    addToGroup:(entryId:string, groupId?:string, groupName?:string) => void;
    findGroup:(groupId?:string, groupName?:string) => IEntryGroup|undefined;
    getGroups():Promise<Array<IEntryGroup>>;
    getGroup(groupId:String):Promise<IEntryGroup|null>;
    isGroupsLoaded:Ref<boolean>;
    removeFromGroup:(entryId:string, groupId?:string, groupName?:string) => void;
    groups:Ref<Array<IEntryGroup>>;
}

export const useEntryGroupingStore = defineStore("entry-grouping-store", ():IEntryGroupingStore => {
   const groups = ref<Array<IEntryGroup>>([]);
   const backend:LocalForage = localForage.createInstance({
        name:"comments",
        version:1.0,
        storeName:"entryGroups"
    });

   const isGroupsLoaded = ref(false);

    async function getGroup(groupId:string) {
        return await backend.getItem<IEntryGroup>(groupId);
    } 

    async function getGroups() {
        const keys = await backend.keys();
        for(let key of keys) {
            const item = await getGroup(key);
            if(item)
            {
                groups.value.push(item);
            }
        }
        
        return groups.value;
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

        if(!groupName) {
            throw new Error("Unable to create a group with a name");
        }

        if(!group) {
            group = EntryGroup.new(groupName, [entryId]);
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
        removeFromGroup,
        groups
   }
});