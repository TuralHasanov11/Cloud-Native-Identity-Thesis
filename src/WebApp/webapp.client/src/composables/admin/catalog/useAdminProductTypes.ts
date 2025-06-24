import { useCatalogStore } from '@/stores/catalog'
import { computed } from 'vue'

export default function useProductTypes() {
  const catalogStore = useCatalogStore()

  return {
    productTypes: computed(() => catalogStore.productTypes),
    getProductTypes: catalogStore.getProductTypes,
  }
}
