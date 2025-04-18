import { defineAsyncComponent, hydrateOnVisible } from 'vue'

export const AsyncBaseFooter = defineAsyncComponent({
  loader: () => import('./BaseFooter.vue'),
  hydrate: hydrateOnVisible(),
})
