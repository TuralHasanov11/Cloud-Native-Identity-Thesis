<script lang="ts" setup>
import useBrands from '@/composables/catalog/useBrands'

const { getBrands } = useBrands()

const { data: brands, isFetching: isFetchingBrands } = await getBrands()
</script>

<template>
  <main class="categories mx-auto">
    <div class="grid grid-cols-2 gap-4 my-6 md:grid-cols-3 lg:gap-8 xl:grid-cols-4">
      <template v-if="!isFetchingBrands">
        <CategoryCard v-for="category in brands" :key="category.id" :category="category" />
      </template>
      <template v-else>
        <CategorySkeletonCard v-for="index in 30" :key="index" />
      </template>
    </div>
  </main>
</template>
