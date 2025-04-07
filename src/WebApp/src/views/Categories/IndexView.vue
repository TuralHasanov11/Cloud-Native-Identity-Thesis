<script lang="ts" setup>
import DefaultLayout from '@/layouts/DefaultLayout.vue'

import useCatalog from '@/composables/useCatalog';

const { brands, getBrands } = useCatalog();

await getBrands();

</script>

<template>
  <DefaultLayout>
    <main class="categories">
      <UContainer>
        <div v-if="brands?.length" class="grid grid-cols-2 gap-4 my-6 md:grid-cols-3 lg:gap-8 xl:grid-cols-4">
          <CategoryCard v-for="category in brands" :key="category.id" :category="category" />
        </div>
      </UContainer>
    </main>
  </DefaultLayout>
</template>
