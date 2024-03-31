import { createApp } from 'vue';
import App from './App.vue';
import { createPinia } from 'pinia';
import PrimeVue from 'primevue/config';
import "primevue/resources/themes/aura-dark-green/theme.css";
import "primeflex/primeflex.css";
import "primeicons/primeicons.css";
import ToastService from 'primevue/toastservice';
import Tooltip from 'primevue/tooltip';


createApp(App)
    .use(createPinia())
    .use(PrimeVue)
    .use(ToastService)
    .directive('tooltip', Tooltip)
    .mount('#app');