export interface IBackend {
    open(name:string, version:number):Promise<Event>;
}

export class Backend implements IBackend {
    open(name: string, version: number): Promise<Event> {
        return new Promise<Event>((resolve, reject) => {
            const response = indexedDB.open(name, version);
            response.addEventListener("success", resolve);
            response.addEventListener("error", reject);
            response.addEventListener("blocked", reject);
            response.addEventListener("upgradeneeded", reject);
        });
    }
}

