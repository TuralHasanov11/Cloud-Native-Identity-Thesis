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
        <div v-if="brands?.length">
          <CategoryCard v-for="category in brands" :key="category.id" :category="category" />
        </div>
      </UContainer>
    </main>
  </DefaultLayout>
</template>
