import { useCatalogStore } from '@/stores/catalog'
import { computed, onMounted } from 'vue'

export default function useAdminBrands() {
  const catalogStore = useCatalogStore()

  onMounted(async () => {
    await catalogStore.getBrands()
  })

  return {
    brands: computed(() => catalogStore.brands),
    getBrands: catalogStore.getBrands,
  }
}
