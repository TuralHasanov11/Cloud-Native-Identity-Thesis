import { useCatalogStore } from '@/stores/catalog'
import type { Product } from '@/types/catalog'
import { computed, ref } from 'vue'
import useBffFetch from './useBffFetch'

const product = ref<Product | null>(null)

const productsPerPage: number = 24

export default function useCatalog() {
  const catalogStore = useCatalogStore()

  async function getProductById(id: string): Promise<void> {
    const { data } = await useBffFetch(`/api/catalog/products/${id}`).json<Product>()
    if (data.value) {
      product.value = data.value
    }
  }

  async function getProductsByIds(ids: string[]): Promise<Product[]> {
    try {
      const params = new URLSearchParams()
      ids.forEach((id) => params.append('ids', id))
      const query = params.toString()
      if (query) {
        const { data } = await useBffFetch<Product[]>(
          `/api/catalog/products/by?${params.toString()}`,
        ).json<Product[]>()
        return data.value ?? []
      }

      return []
    } catch (error) {
      console.error('Error fetching products by IDs:', error)
      return []
    }
  }

  return {
    popularProducts: computed(() => catalogStore.popularProducts),
    brands: computed(() => catalogStore.brands),
    products: computed(() => catalogStore.products),
    productTypes: computed(() => catalogStore.productTypes),
    product,
    getProducts: catalogStore.getProducts,
    getBrands: catalogStore.getBrands,
    getProductTypes: catalogStore.getProductTypes,
    getProductById,
    getProductsByIds,
    productsPerPage,
  }
}
