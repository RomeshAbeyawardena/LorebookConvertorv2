import { ref, Ref } from "vue";
import { defineStore } from "pinia";
export interface ILoaderStore {
    isLoading:Ref<boolean>;
    isLoaded:() => Promise<boolean>;
};

export const useLoaderStore = defineStore("loader-store", (): ILoaderStore => {
    const isLoading = ref(false);
    
    const maximumAttempts = 30;
    const timeout = 1000;
    function awaiter(currentAttempt:number, resolve:(v:boolean) => void, reject:(error:any) => void) {
        if(currentAttempt >= maximumAttempts) {
            reject(new Error("maximum attempts have been exceeded"));
        }
        
        if(isLoading.value) {
            window.setTimeout(() => awaiter(currentAttempt+1, resolve, reject), timeout);
            return;
        }
        
        resolve(!isLoading.value);
    }

    function isLoaded() {
        return new Promise<boolean>((resolve, reject) => {
            window.setTimeout(() => awaiter(0, resolve, reject), timeout)
        });
    }

    return {
        isLoading,
        isLoaded
    }
});