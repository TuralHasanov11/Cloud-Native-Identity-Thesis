<script setup lang="ts">
import useBrands from '@/composables/useBrands'
import { onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
const { t } = useI18n()
const { brands, getBrands } = useBrands()

onMounted(async () => {
  await getBrands()
})

</script>

<template>
  <main id="home">
    <HeroBanner />

    <div class="container mx-auto">
      <section class="my-16">
        <div class="flex items-end justify-between">
          <h2 class="text-lg font-semibold md:text-2xl">
            {{ t('messages.shop.shopByCategory') }}
          </h2>
          <RouterLink class="text-primary" to="/categories">{{
            t('messages.general.viewAll')
          }}</RouterLink>
        </div>
        <div class="grid justify-center grid-cols-2 gap-4 mt-8 md:grid-cols-3 lg:grid-cols-6">
          <CategoryCard v-for="category in brands" :key="category.id" :category="category" />
        </div>
      </section>
    </div>
  </main>
</template>
