import { defineStore } from "pinia";
import { Ref, ref } from "vue";
import { useToast } from "primevue/usetoast";
import { INotification } from "../models/notification";

export interface INotificationStore {
    notification: Ref<INotification>;
    set(notification:INotification): void;
}

export const useNotificationStore = defineStore("notification", 
    ():INotificationStore => {
    const notification = ref<INotification>({
        title: "",
        lifetime: 1000,
        message: "",
        severity: "info",
        visible: false
    });

    const toast = useToast();

    function set(n: INotification) {
         notification.value = n;
         notification.value.visible = true;
         
         if(!n.visible){
            toast.removeAllGroups();
            return;
         }

         toast.add({
            severity: n.severity,
            summary: n.title,
            detail: n.message,
            life: n.lifetime
         });
    }

    return {
        set,
        notification
    }
});