import useBffFetch from '@/composables/useBffFetch'
import type { ProductType } from '@/types/catalog'
import type { App } from 'vue'

export interface ProductTypeRepository {
  list: () => Promise<ProductType[]>
}

export const productTypeRepository = ($fetch: typeof useBffFetch): ProductTypeRepository => ({
  async list() {
    const { data } = await $fetch('/api/catalog/product-types').json<ProductType[]>()
    return data.value || []
  },
})

export const productTypeRepositoryName = productTypeRepository.name

export default {
  install: (app: App<unknown>) => {
    app.provide(productTypeRepositoryName, productTypeRepository(useBffFetch))
  },
}
