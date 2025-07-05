import './assets/main.css'

import { createPinia } from 'pinia'
import { createApp } from 'vue'

import Aura from '@primeuix/themes/aura'
import PrimeVue from 'primevue/config'
import ToastService from 'primevue/toastservice'
import { createI18n } from 'vue-i18n'
import App from './App.vue'
import auth from './plugins/auth'
import router from './router'
import en from './locales/en.json'
import de from './locales/de.json'

const app = createApp(App)

app.use(createPinia()).use(auth)
app.use(
  createI18n({
    legacy: false,
    locale: 'en',
    fallbackLocale: 'en',
    messages: {
      en: en,
      de: de,
    },
  }),
)
app.use(router)
app.use(PrimeVue, {
  theme: {
    preset: Aura,
    options: {
      cssLayer: {
        name: 'primevue',
        order: 'theme, base, primevue',
      },
    },
  },
})
app.use(ToastService)

app.config.errorHandler = (err, vm, info) => {
  console.error('Error:', err)
  console.error('Vue instance:', vm)
  console.error('Info:', info)
}

app.config.warnHandler = (msg, vm, trace) => {
  console.warn('Warning:', msg)
  console.warn('Vue instance:', vm)
  console.warn('Trace:', trace)
}

app.config.performance = true

app.mount('#app')
