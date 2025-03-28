<script lang="ts" setup>
const { categories } = useCatalog();

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
        <UBlogPosts v-if="categories?.length">
          <UBlogPost
            v-for="(category, index) in categories"
            :key="category.id"
            :to="`/product-category/${category.slug}`"
            :title="category.name"
            :orientation="index === 0 ? 'horizontal' : 'vertical'"
            :class="[index === 0 && 'col-span-full']"
            variant="naked"
            :ui="{
              description: 'line-clamp-2',
            }"
          />
        </UBlogPosts>
      </UPageBody>
    </UContainer>
  </main>
</template>
