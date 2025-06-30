import type { GetProductsRequest, Product } from '@/types/catalog'
import useBffFetch from '../useBffFetch'
import type { PaginationResponse } from '@/types/pagination'

export default function useProducts() {
  return {
    getProducts(payload?: GetProductsRequest) {
      const params = new URLSearchParams()

      if (payload?.brand) {
        params.append('brand', payload.brand)
      }

      if (payload?.productType) {
        params.append('type', payload.productType)
      }

      if (payload?.name) {
        params.append('name', payload.name)
      }

      if (payload?.pageSize) {
        params.append('pageSize', payload.pageSize.toString())
      }

      if (payload?.pageCursor) {
        params.append('pageCursor', payload.pageCursor)
      }

      return useBffFetch(`/api/catalog/products?${params.toString()}`).json<PaginationResponse<Product, string>>()
    },

    getProductById(id: string) {
      return useBffFetch(`/api/catalog/products/${id}`).json<Product>()
    },

    getProductsByIds(ids: string[]) {
      const params = new URLSearchParams()
      ids.forEach((id) => params.append('ids', id))
      return useBffFetch<Product[]>(`/api/catalog/products/by?${params.toString()}`).json<Product[]>()
    },
  }
}
