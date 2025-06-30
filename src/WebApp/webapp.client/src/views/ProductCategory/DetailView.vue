<script setup lang="ts">
import ProductFilters from '@/components/Catalog/ProductFilters.vue'
import useProducts from '@/composables/catalog/useProducts'
import { watch } from 'vue'
import { useRoute } from 'vue-router'

const { getProducts } = useProducts()
const route = useRoute()

const { data: productsContainer } = await getProducts({ brand: route.params.slug as string })

watch(
  () => route.params,
  async () => {
    if (route.name !== 'product-category') return

    await getProducts({ brand: route.params.slug as string })
  },
)
</script>

<template>
  <main class="container mx-auto">
    <ProductFilters :hide-categories="true" />

    <div class="w-full">
      <div class="flex items-center justify-between w-full gap-4 mt-8 md:gap-8">
        <ProductResultCount :productCount="productsContainer?.data?.length" />
        <ShowFilterTrigger class="md:hidden" />
      </div>
      <ProductGrid :products="productsContainer?.data" />
    </div>
  </main>
</template>
