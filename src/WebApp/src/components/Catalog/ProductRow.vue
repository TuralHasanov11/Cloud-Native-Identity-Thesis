<script setup lang="ts">
import type { Product } from '@/types/catalog';

defineProps<{
  products: Product[]
}>();
</script>

<template>
  <div v-if="products" class="grid gap-8">
    <ProductCard v-for="(product, i) in products" :key="product.id" class="w-full" :product="product" :index="i" :class="{
      hidden: i === products.length - 1,
      'lg:block': i === products.length - 1,
    }" />
  </div>
</template>
