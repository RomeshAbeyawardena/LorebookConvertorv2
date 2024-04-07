import { defineStore } from "pinia";
import { Ref, ref } from "vue";
import { IEntryGroup } from "../models/EntryGroup";

export interface IEntryGroupingStore {
    addToGroup:(entryId:string, groupId?:string, groupName?:string) => void;
    findGroup:(groupId?:string, groupName?:string) => IEntryGroup|undefined;
    removeFromGroup:(entryId:string, groupId?:string, groupName?:string) => void;
    groups:Ref<Array<IEntryGroup>>;
}

export const useEntryGroupingStore = defineStore("entry-grouping-store", ():IEntryGroupingStore => {
   const groups = ref<Array<IEntryGroup>>([]);

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
            group = {
                entryIds: [entryId],
                groupId: "",
                name: groupName
            }
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
        findGroup,
        removeFromGroup,
        groups
   }
});