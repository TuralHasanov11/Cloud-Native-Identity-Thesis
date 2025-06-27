import { useCatalogStore } from '@/stores/catalog'
import { computed } from 'vue'

export default function useAdminBrands() {
  const catalogStore = useCatalogStore()

  return {
    brands: computed(() => catalogStore.brands),
    getBrands: catalogStore.getBrands,
  }
}
