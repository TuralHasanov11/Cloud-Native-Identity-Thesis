import useBffFetch from '@/composables/useBffFetch'
import { PAGINATED_PRODUCTS_NULL_OBJECT, PRODUCT_NULL_OBJECT, type CreateProductRequest, type Product } from '@/types/catalog'
import { HttpStatusCode } from '@/types/common'
import type { PaginationResponse } from '@/types/pagination'
import { Err, Ok } from 'ts-results'
import { ref } from 'vue'

export default function useAdminProducts() {
  const products = ref<PaginationResponse<Product, string>>(PAGINATED_PRODUCTS_NULL_OBJECT)
  const product = ref<Product>(PRODUCT_NULL_OBJECT)

  async function getProducts(): Promise<void> {
    const { data } = await useBffFetch(`/api/catalog/products`).json<PaginationResponse<Product, string>>()
    if (data.value) {
      products.value = data.value
    }
  }

  async function getProduct(id: string) {
    const { data } = await useBffFetch(`/api/catalog/products/${id}`).json<Product>()
    if (data.value) {
      product.value = data.value
      return Ok.EMPTY
    }
    return new Err('Product not found')
  }

  async function createProduct(data: CreateProductRequest) {
    const response = await useBffFetch('/api/catalog/products').post(data)

    if (response.statusCode.value === HttpStatusCode.Created) {
      return Ok.EMPTY
    } else {
      return new Err(response.error.value)
    }
  }

  async function updateProduct(id: string, data: CreateProductRequest) {
    const response = await useBffFetch(`/api/catalog/products/${id}`).put(data)
    if (response.statusCode.value === HttpStatusCode.NoContent) {
      return Ok.EMPTY
    } else {
      return new Err(response.error.value)
    }
  }

  async function deleteProduct(id: string) {
    const response = await useBffFetch(`/api/catalog/products/${id}`).delete()
    if (response.statusCode.value === HttpStatusCode.NoContent) {
      return Ok.EMPTY
    } else {
      return new Err(response.error.value)
    }
  }

  return {
    products,
    product,
    getProducts,
    getProduct,
    createProduct,
    updateProduct,
    deleteProduct,
  }
}
