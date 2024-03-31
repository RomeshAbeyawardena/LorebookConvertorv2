
export interface INotification {
    message:string;
    severity:"error" | "success" | "secondary" | "info" | "contrast" | "warn";
    lifetime:number;
    title:string;
    visible:boolean;
}