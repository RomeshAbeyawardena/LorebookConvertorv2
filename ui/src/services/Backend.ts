export interface IBackend {
    db?:IDBDatabase;
    configure(entries:Array<[s:string, parameters:IDBObjectStoreParameters, configure:(store:IDBObjectStore) => void ]>):void;
    get<T>(store:IDBObjectStore) : Promise<Array<T>>;
    open(name:string, version:number):Promise<Event>;
    store(storeName:string, mode?:IDBTransactionMode):IDBObjectStore|undefined;
    put<T>(store:IDBObjectStore, items:Array<T>, key:(item:T) => any) : Promise<Event>;
    close():Promise<Event>;
}

export class Backend implements IBackend {
    private openRequest?:IDBOpenDBRequest;
    private entries:Array<[s:string, parameters:IDBObjectStoreParameters, configure:(store:IDBObjectStore) => void ]> = [];

    db?:IDBDatabase;

    get<T>(store:IDBObjectStore) : Promise<Array<T>> {
        return new Promise<Array<T>>((resolve, reject) => {
            const request = store.getAll();
            request.addEventListener("success", () => {
                resolve(request.result);
            });

            request.addEventListener("error", () => {
                reject(request.error);
            });
        });
        
    }

    open(name: string, version: number): Promise<Event> {
        return new Promise<Event>((resolve, reject) => {
            if(this.openRequest && this.db) {
                return resolve(new Event("open", { "composed":true }));
            }

            this.openRequest = indexedDB.open(name, version);
            this.openRequest.addEventListener("success", e => this.onSuccess(e, resolve));
            this.openRequest.addEventListener("error", reject);
            this.openRequest.addEventListener("blocked", reject);
            this.openRequest.addEventListener("upgradeneeded", e => this.onUpgrade(e, resolve));
        });
    };

    close() : Promise<Event> {
        return new Promise<Event>((resolve, reject) => {
            if(this.db)
            {
                this.db.addEventListener("close", resolve);
                this.db.addEventListener("error", reject);
                this.db.close();
                this.db = undefined;
            }
            else 
            {
                resolve(new Event("nodatabasetoclose"));
            }

            if(this.openRequest)
            {
                this.openRequest = undefined;
            }
        });
    }

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

    store(storeName:string, mode?:IDBTransactionMode):IDBObjectStore|undefined {
        return this.db?.transaction(storeName, mode).objectStore(storeName);
    }

    put<T>(store:IDBObjectStore, items:Array<T>, key:(item:T) => any) : Promise<Event> {
        return new Promise((resolve, reject) => {
            items.forEach(c => {
                const valid = store.put(c, key(c));
                valid?.addEventListener("success", resolve);
                valid?.addEventListener("error", reject)
            });
        });
    }
}

