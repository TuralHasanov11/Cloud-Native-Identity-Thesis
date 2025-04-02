<script setup lang="ts">
import type { Brand } from '~/types/catalog';

// const { FALLBACK_IMG } = useHelpers();

const {category, imagesLoading = 'eager'} = defineProps<{
  category: Brand,
  imageLoading?: 'lazy' | 'eager'
}>(
);
</script>

<template>
  <UPageCard
      :title="category.name"
      :to="`/product-category/${decodeURIComponent(category.id)}`"
    />
  
</template>