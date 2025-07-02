import type { Brand } from '@/types/catalog'
import useBffFetch from '../useBffFetch'

export default function useBrands() {
  return {
    getBrands() {
      return useBffFetch('/api/catalog/brands').json<Brand[]>()
    },
  }
}
