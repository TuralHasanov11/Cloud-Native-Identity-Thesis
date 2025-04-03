<script setup lang="ts">
import type { Product } from '~/types/catalog';

const { storeSettings } = useAppConfig();

const {product} = defineProps<{
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
//       const hasMatchingSlug = paColor.value.some((color) => variation.slug?.includes(color));
//       return hasMatchingAttributes || hasMatchingSlug;
//     });
//     if (activeColorImage?.length) return activeColorImage[0]?.image?.producCardSourceUrl || activeColorImage[0]?.image?.sourceUrl || mainImage.value;
//   }
//   return mainImage.value;
// });
</script>

<template>
  <div class="relative group">
    <NuxtLink :to="`/products/${decodeURIComponent(product.slug)}`" :title="product.name">
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
    </NuxtLink>
    <div class="p-2">
      <StarRating v-if="storeSettings.showReviews" :rating="5" :count="5" />
      <NuxtLink :to="`/product/${decodeURIComponent(product.slug)}`" :title="product.name">
        <h2 class="mb-2 font-light leading-tight group-hover:text-primary">{{ product.name }}</h2>
      </NuxtLink>
      <ProductPrice class="text-sm" :sale-price="product.price" :regular-price="product.price" />
    </div>
  </div>
</template>
