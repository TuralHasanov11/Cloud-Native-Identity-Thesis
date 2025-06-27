import useBffFetch from '@/composables/useBffFetch'
import type { Brand } from '@/types/catalog'
import type { App } from 'vue'

export interface BrandRepository {
  list: () => Promise<Brand[]>
}

export const brandRepository = ($fetch: typeof useBffFetch): BrandRepository => ({
  async list() {
    const { data } = await $fetch('/api/catalog/brands').json<Brand[]>()
    return data.value || []
  },
})

export const brandRepositoryName = brandRepository.name

export default {
  install: (app: App<unknown>) => {
    app.provide(brandRepositoryName, brandRepository(useBffFetch))
  },
}
