<script lang="ts" setup>
import { onMounted } from 'vue'
import ProductFilters from '@/components/Catalog/ProductFilters.vue'
import useCatalog from '@/composables/useCatalog'

const { products, getProducts } = useCatalog()

onMounted(async () => {
  await getProducts()
})
</script>

<template>
  <main class="products">
    <div class="container mx-auto">
      <ProductFilters :hide-categories="true" />

      <div class="w-full">
        <div class="flex items-center justify-between w-full gap-4 mt-8 md:gap-8">
          <ProductResultCount />
          <ShowFilterTrigger class="md:hidden" />
        </div>
        <ProductGrid :products="products.data" />
      </div>
    </div>
  </main>
</template>
