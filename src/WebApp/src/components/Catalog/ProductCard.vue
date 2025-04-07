<script setup lang="ts">
import type { Product } from '@/types/catalog';
import { useImage } from '@vueuse/core';

const { product } = defineProps<{
  product: Product,
  index: number
}>();

const imgWidth = 280;
const imgHeight = Math.round(imgWidth * 1.125);
const { FALLBACK_IMG } = useHelpers();

const { isLoading: isImageLoading } = useImage({ src: FALLBACK_IMG })

</script>

<template>
  <div class="relative group">
    <RouterLink :to="`/products/${decodeURIComponent(product.id)}`" :title="product.name">
      <SaleBadge :product class="absolute top-2 right-2" />
      <USkeleton v-if="isImageLoading" class="rounded-full" :width="imgWidth" :height="imgHeight" />
      <img v-else :width="imgWidth" :height="imgHeight" :src="FALLBACK_IMG" :alt="product.name" :title="product.name"
        :loading="index <= 3 ? 'eager' : 'lazy'" :sizes="`sm:${imgWidth / 2}px md:${imgWidth}px`"
        class="rounded-lg object-top object-cover w-full aspect-9/8" placeholder-class="blur-xl" />
    </RouterLink>
    <div class="p-2">
      <RouterLink :to="`/products/${decodeURIComponent(product.id)}`" :title="product.name">
        <h2 class="mb-2 font-light leading-tight group-hover:text-primary">{{ product.name }}</h2>
      </RouterLink>
      <ProductPrice class="text-sm" :sale-price="product.price" :regular-price="product.price" />
    </div>
  </div>
</template>
