import { useCatalogStore } from '@/stores/catalog'
import type { Product } from '@/types/catalog'
import { computed, ref } from 'vue'

const product = ref<Product | null>(null)

const productsPerPage: number = 24

export default function useCatalog() {
  const catalogStore = useCatalogStore()

  async function getProductById(id: string): Promise<void> {
    console.log('getProductById', id)
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
    productsPerPage,
  }
}
