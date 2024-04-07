import { v4 } from "uuid";
export interface IEntryGroup {
    groupId:string;
    entryIds:Array<string>;
    name:string;
}

export class EntryGroup {
    public static new(name:string, entryIds?:Array<string>, groupId?:string) :IEntryGroup {
        return {
            entryIds: entryIds ?? [],
            groupId: groupId ?? v4(),
            name: name
        }
    }
}