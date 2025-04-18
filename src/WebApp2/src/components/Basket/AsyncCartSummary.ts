import { defineAsyncComponent, hydrateOnVisible } from 'vue'

export const AsyncCartSummary = defineAsyncComponent({
  loader: () => import('./CartSummary.vue'),
  hydrate: hydrateOnVisible(),
})
