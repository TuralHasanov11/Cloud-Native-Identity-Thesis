<script setup lang="ts">
import DefaultLayout from '@/layouts/DefaultLayout.vue'

const { brands, getBrands } = useCatalog()

await getBrands()

</script>

<template>
  <DefaultLayout>
    <main id="home">
      <HeroBanner />

      <UContainer>
        <section class="my-16">
          <div class="flex items-end justify-between">
            <h2 class="text-lg font-semibold md:text-2xl">{{ $t('messages.shop.shopByCategory') }}</h2>
            <RouterLink class="text-primary" to="/categories">{{ $t('messages.general.viewAll') }}</RouterLink>
          </div>
          <div class="grid justify-center grid-cols-2 gap-4 mt-8 md:grid-cols-3 lg:grid-cols-6">
            <CategoryCard v-for="category in brands" :key="category.id" :category="category" />
          </div>
        </section>
      </UContainer>
    </main>
  </DefaultLayout>
</template>
