<script lang="ts" setup>
import DefaultLayout from '@/layouts/DefaultLayout.vue'

import ProductFilters from '@/components/Catalog/ProductFilters.vue';
import useCatalog from '@/composables/useCatalog';

const { products, getProducts } = useCatalog();

await getProducts();

</script>

<template>
  <DefaultLayout>
    <main class="products">
      <UContainer>
        <ProductFilters :hide-categories="true" />

        <div class="w-full">
          <div class="flex items-center justify-between w-full gap-4 mt-8 md:gap-8">
            <ProductResultCount />
            <OrderByDropdown class="hidden md:inline-flex" />
            <ShowFilterTrigger class="md:hidden" />
          </div>
          <ProductGrid :products="products.data" />
        </div>
      </UContainer>
    </main>
  </DefaultLayout>
</template>
