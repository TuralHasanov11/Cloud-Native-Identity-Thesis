import useBffFetch from '@/composables/useBffFetch'
import type { ProductType } from '@/types/catalog'

export default function useAdminProductTypes() {
  return {
    getProductTypes() {
      return useBffFetch('/api/catalog/product-types').json<ProductType[]>()
    },
  }
}
