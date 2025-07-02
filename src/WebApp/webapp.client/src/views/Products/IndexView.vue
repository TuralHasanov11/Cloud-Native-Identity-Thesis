<script lang="ts" setup>
import ProductFilters from '@/components/Catalog/ProductFilters.vue'
import useProducts from '@/composables/catalog/useProducts'

const { getProducts } = useProducts()

const { data: productsContainer } = await getProducts()
</script>

<template>
  <main class="products">
    <div class="container mx-auto">
      <ProductFilters :hide-categories="true" />

      <div class="w-full">
        <div class="flex items-center justify-between w-full gap-4 mt-8 md:gap-8">
          <ProductResultCount :productCount="productsContainer?.data?.length" />
          <ShowFilterTrigger class="md:hidden" />
        </div>
        <ProductGrid v-if="productsContainer" :products="productsContainer.data" />
      </div>
    </div>
  </main>
</template>
