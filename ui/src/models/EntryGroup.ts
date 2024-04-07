import { v4 } from "uuid";
export interface IEntryGroup {
    entryIds:Array<string>;
    groupId:string;
    name:string;
    storyId:string;
}

export class EntryGroup {
    public static new(storyId:string, name:string, entryIds?:Array<string>, groupId?:string) :IEntryGroup {
        return {
            entryIds: entryIds ?? [],
            groupId: groupId ?? v4(),
            storyId: storyId,
            name: name
        }
    }
}