import { ICategory } from "./category"
import { IEntry } from "./entry"
export interface ISearchIndex {
    id:string;
    keys:Array<string>;
    entry?:IEntry;
    category?:ICategory;
    summary?:string;
    title?:string;
}