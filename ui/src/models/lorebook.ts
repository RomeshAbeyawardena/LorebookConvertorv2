import { IEntry } from "./entry";
import { ICategory } from "./category";
import { ILorebookGroup } from "./groups";

export interface ILorebook {
    Entries:Array<IEntry>;
    Categories:Array<ICategory>;
    Groupings:Array<ILorebookGroup>;
}
