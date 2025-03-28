<script setup lang="ts">
const { categoryProducts, getProducts } = useCatalog();
const { storeSettings } = useAppConfig();
const route = useRoute();

await getProducts({category: route.params.slug as string});

watch(
  () => route.query,
  async () => {
    if (route.name !== 'product-category-slug') return;

    await getProducts({category: route.params.slug as string});
  },
);

useSeoMeta({
  title: "Categories",
  ogTitle: "Categories",
  description: "All product categories",
  ogDescription: "All product categories",
});

</script>

<template>
  <main class="container">
    <UContainer>
      <!-- <UPageHeader
      v-bind="page"
      class="py-[50px]"
    /> -->

      <UPageBody>
        <!-- <Filters v-if="storeSettings.showFilters" :hide-categories="true" /> -->

        <div class="w-full">
            <div class="flex items-center justify-between w-full gap-4 mt-8 md:gap-8">
                <ProductResultCount />
                <!-- <OrderByDropdown v-if="storeSettings.showOrderByDropdown" class="hidden md:inline-flex" /> -->
                <!-- <ShowFilterTrigger v-if="storeSettings.showFilters" class="md:hidden" /> -->
            </div>
            <ProductGrid :products="categoryProducts" />
        </div>
        
      </UPageBody>
    </UContainer>
  </main>
</template>
