import { ICategory } from "./category";

export interface IEntry {
    Id:string;
    DisplayName:string;
    Text:string;
    Keys:Array<string>;
    CategoryId:string;
    Category:ICategory;
} 

export class EntryConstructor implements IEntry {
    Id:string = "";
    DisplayName:string = "";
    Text:string = "";
    Keys:Array<string> = [];
    CategoryId:string = "";
    Category:ICategory = { Id:"", Name:"" };
}