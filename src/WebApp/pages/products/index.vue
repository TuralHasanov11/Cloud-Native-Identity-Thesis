<script lang="ts" setup>
const { siteImage, storeSettings } = useAppConfig();
const { products, getProducts } = useCatalog();

await getProducts();

useSeoMeta({
  title: `Products`,
  ogTitle: "Products",
  description: "Products",
  ogDescription: "Products",
  ogImage: siteImage,
  twitterCard: `summary_large_image`,
});
</script>

<template>
  <main class="products">
    <UContainer>
      <!-- <UPageHeader
      v-bind="page"
      class="py-[50px]"
    /> -->

      <UPageBody>
        <Filters v-if="storeSettings.showFilters" :hide-categories="true" />

        <div class="w-full">
          <div
            class="flex items-center justify-between w-full gap-4 mt-8 md:gap-8"
          >
            <ProductResultCount />
            <OrderByDropdown v-if="storeSettings.showOrderByDropdown" class="hidden md:inline-flex" />
            <ShowFilterTrigger v-if="storeSettings.showFilters" class="md:hidden" />
          </div>
          <ProductGrid :products="products.data" />
        </div>
      </UPageBody>
    </UContainer>
  </main>
</template>
