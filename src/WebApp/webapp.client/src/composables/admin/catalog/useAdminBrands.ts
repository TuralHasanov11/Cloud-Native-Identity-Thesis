import useBffFetch from '@/composables/useBffFetch'
import type { Brand } from '@/types/catalog'

export default function useAdminBrands() {
  return {
    getBrands() {
      return useBffFetch('/api/catalog/brands').json<Brand[]>()
    },
  }
}
