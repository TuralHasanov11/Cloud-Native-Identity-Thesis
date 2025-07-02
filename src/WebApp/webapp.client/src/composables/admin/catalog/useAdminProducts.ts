import useBffFetch from '@/composables/useBffFetch'
import { type CreateOrUpdateProductRequest, type Product } from '@/types/catalog'
import { HttpStatusCode } from '@/types/common'
import type { PaginationResponse } from '@/types/pagination'
import { Err, Ok } from 'ts-results'

export default function useAdminProducts() {
  return {
    getProducts() {
      return useBffFetch(`/api/catalog/products`).json<PaginationResponse<Product, string>>()
    },

    getProduct(id: string) {
      return useBffFetch(`/api/catalog/products/${id}`).json<Product>()
    },

    async createProduct(data: CreateOrUpdateProductRequest) {
      const response = await useBffFetch('/api/catalog/products').post(data)

      if (response.statusCode.value === HttpStatusCode.Created) {
        return Ok.EMPTY
      } else {
        return new Err(response.error.value)
      }
    },

    async updateProduct(id: string, data: CreateOrUpdateProductRequest) {
      const response = await useBffFetch(`/api/catalog/products/${id}`).put(data)
      if (response.statusCode.value === HttpStatusCode.NoContent) {
        return Ok.EMPTY
      } else {
        return new Err(response.error.value)
      }
    },

    async deleteProduct(id: string) {
      const response = await useBffFetch(`/api/catalog/products/${id}`).delete()
      if (response.statusCode.value === HttpStatusCode.NoContent) {
        return Ok.EMPTY
      } else {
        return new Err(response.error.value)
      }
    },
  }
}
