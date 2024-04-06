import { DateTime } from "luxon";

export interface IDateService {
    format(date:Date, format?:string):string;
}

export class DateService implements IDateService {
    format(date:Date, format?:string):string {
        const dateTime = DateTime.fromJSDate(date)
        return format 
            ? dateTime.toFormat(format)
            : dateTime.toLocaleString(DateTime.DATETIME_MED);
    }
}