export interface IBackend {
    db?:IDBDatabase;
    configure(entries:Array<[s:string, parameters:IDBObjectStoreParameters, configure:(store:IDBObjectStore) => void ]>):void;
    open(name:string, version:number):Promise<Event>;
    store(name:string, storeName:string):IDBObjectStore|undefined
}

export class Backend implements IBackend {
    private openRequest?:IDBOpenDBRequest;
    private entries:Array<[s:string, parameters:IDBObjectStoreParameters, configure:(store:IDBObjectStore) => void ]> = [];

    db?:IDBDatabase;
    open(name: string, version: number): Promise<Event> {
        return new Promise<Event>((resolve, reject) => {
            this.openRequest = indexedDB.open(name, version);
            this.openRequest.addEventListener("success", e => this.onSuccess(e, resolve));
            this.openRequest.addEventListener("error", reject);
            this.openRequest.addEventListener("blocked", reject);
            this.openRequest.addEventListener("upgradeneeded", e => this.onUpgrade(e, resolve));
        });
    };

    configure(entries: Array<[s: string, parameters: IDBObjectStoreParameters, configure:(store:IDBObjectStore) => void ]>): void {
        this.entries = entries;
    }

    onSuccess(ev:Event, resolve:(event: Event) => void) {
       this.db = this.openRequest?.result;
       resolve(ev);
    };

    onUpgrade(ev:Event, upgrade:(event: Event) => void) {
        this.db = this.openRequest?.result;
        
        const _this = this;
            this.entries.forEach(f => {
                const store = _this.db?.createObjectStore(f[0], f[1]);
                if(store)
                {
                    f[2](store);
                }
            });

        upgrade(ev);
    }

    store(name:string, storeName:string, mode?:IDBTransactionMode):IDBObjectStore|undefined {
        return this.db?.transaction(name, mode).objectStore(storeName);
    }
}

