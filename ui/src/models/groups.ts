import { ICategory } from "./category";
import { IEntry } from "./entry";

export interface ILorebookGroup {
    CategoryId:string;
    Category:ICategory;
    Entries:Array<IEntry>;
    isGroup:boolean;
}