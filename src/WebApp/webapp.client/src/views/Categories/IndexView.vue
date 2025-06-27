<script lang="ts" setup>
import useBrands from '@/composables/useBrands'

const { brands, getBrands } = useBrands()

onMounted(async () => {
  await getBrands()
})

</script>

<template>
  <main class="categories mx-auto">
    <div
      v-if="brands?.length"
      class="grid grid-cols-2 gap-4 my-6 md:grid-cols-3 lg:gap-8 xl:grid-cols-4"
    >
      <CategoryCard v-for="category in brands" :key="category.id" :category="category" />
    </div>
  </main>
</template>
