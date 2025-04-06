<script setup lang="ts">
import type { Product } from '@/types/catalog';

const { product } = defineProps<{
  product: Product,
  index: number
}>();

// const imgWidth = 280;
// const imgHeight = Math.round(imgWidth * 1.125);

// const mainImage = computed<string>(() => product?.image?.producCardSourceUrl || product?.image?.sourceUrl || '/images/placeholder.jpg');
// const imagetoDisplay = computed<string>(() => {
//   if (paColor.value.length) {
//     const activeColorImage = product?.variations?.nodes.filter((variation) => {
//       const hasMatchingAttributes = variation.attributes?.nodes.some((attribute) => paColor.value.some((color) => attribute?.value?.includes(color)));
//       const hasMatchingId = paColor.value.some((color) => variation.id?.includes(color));
//       return hasMatchingAttributes || hasMatchingId;
//     });
//     if (activeColorImage?.length) return activeColorImage[0]?.image?.producCardSourceUrl || activeColorImage[0]?.image?.sourceUrl || mainImage.value;
//   }
//   return mainImage.value;
// });
</script>

<template>
  <div class="relative group">
    <RouterLink :to="`/products/${decodeURIComponent(product.id)}`" :title="product.name">
      <SaleBadge :product class="absolute top-2 right-2" />
      <!-- <NuxtImg
        v-if="imagetoDisplay"
        :width="imgWidth"
        :height="imgHeight"
        :src="imagetoDisplay"
        :alt="product.image?.altText || product.name || 'Product image'"
        :title="product.image?.title || product.name"
        :loading="index <= 3 ? 'eager' : 'lazy'"
        :sizes="`sm:${imgWidth / 2}px md:${imgWidth}px`"
        class="rounded-lg object-top object-cover w-full aspect-9/8"
        placeholder
        placeholder-class="blur-xl" /> -->
    </RouterLink>
    <div class="p-2">
      <RouterLink :to="`/product/${decodeURIComponent(product.id)}`" :title="product.name">
        <h2 class="mb-2 font-light leading-tight group-hover:text-primary">{{ product.name }}</h2>
      </RouterLink>
      <ProductPrice class="text-sm" :sale-price="product.price" :regular-price="product.price" />
    </div>
  </div>
</template>
