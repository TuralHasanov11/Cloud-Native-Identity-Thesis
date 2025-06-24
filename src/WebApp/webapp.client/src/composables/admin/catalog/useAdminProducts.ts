import useBffFetch from '@/composables/useBffFetch'
import { PAGINATED_PRODUCTS_NULL_OBJECT, type Product } from '@/types/catalog'
import type { PaginationResponse } from '@/types/pagination'
import { ref } from 'vue'

export default function useAdminProducts() {
  const products = ref<PaginationResponse<Product, string>>(PAGINATED_PRODUCTS_NULL_OBJECT)

  async function getProducts(): Promise<void> {
    const { data } = await useBffFetch(`/api/catalog/products`).json<PaginationResponse<Product, string>>()
    if (data.value) {
      products.value = data.value
    }
  }

  return {
    products,
    getProducts,
  }
}
