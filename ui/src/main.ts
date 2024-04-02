import { createApp } from 'vue';
import App from './App.vue';
import { createPinia } from 'pinia';
import PrimeVue from 'primevue/config';
import "primevue/resources/themes/aura-dark-green/theme.css";
import "primeflex/primeflex.css";
import "primeicons/primeicons.css";

import "./sass/index.scss";
import ToastService from 'primevue/toastservice';
import Tooltip from 'primevue/tooltip';


const app = createApp(App);

app
    .use(createPinia())
    .use(PrimeVue)
    .use(ToastService)
    .directive('tooltip', Tooltip)
    .mount('#app');