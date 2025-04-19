<script setup lang="ts">
import DefaultLayout from '@/layouts/DefaultLayout.vue'

import ProductFilters from '@/components/Catalog/ProductFilters.vue';
import useCatalog from '@/composables/useCatalog';
import { watch } from 'vue';
import { useRoute } from 'vue-router';

const { products, getProducts } = useCatalog();
const route = useRoute();

await getProducts({ brand: route.params.slug as string });

watch(
  () => route.params,
  async () => {
    if (route.name !== 'product-category') return;

    await getProducts({ brand: route.params.slug as string });
  },
);
</script>

<template>
  <DefaultLayout>
    <main class="container">
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
