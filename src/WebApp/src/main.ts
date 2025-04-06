import './assets/main.css'
import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { createI18n } from 'vue-i18n'

import ui from '@nuxt/ui/vue-plugin'

import App from './App.vue'
import router from './router'
import auth from './plugins/auth'

const app = createApp(App)

app.use(createPinia()).use(auth)
app.use(
  createI18n({
    locale: 'de',
    fallbackLocale: 'en',
    messages: {
      en: {
        message: {
          hello: 'hello world',
        },
      },
      de: {
        message: {
          hello: 'hallo welt',
        },
      },
    },
  }),
)
app.use(router)
app.use(ui)

app.mount('#app')
