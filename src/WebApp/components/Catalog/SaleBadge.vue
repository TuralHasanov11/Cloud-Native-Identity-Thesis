<script setup lang="ts">
import type { Product } from '~/types/catalog';

const { t } = useI18n();
const { product } = defineProps<{
  product: Product
}>();

const { storeSettings } = useAppConfig();

const salePercentage = computed((): string => {
  if (!product?.price || !product?.price) return '';
  const salePrice = product?.price;
  const regularPrice = product?.price;
  return Math.round(((salePrice - regularPrice) / regularPrice) * 100) + ` %`;
});

const showSaleBadge = computed(() => product.price && storeSettings.saleBadge !== 'hidden');

const textToDisplay = computed(() => {
  if (storeSettings?.saleBadge === 'percent') return salePercentage.value;
  return t('messages.shop.onSale') ? t('messages.shop.onSale') : 'Sale';
});
</script>

<template>
  <span v-if="showSaleBadge" class="red-badge">{{ textToDisplay }}</span>
</template>

<style lang="postcss" scoped>
.red-badge {
  @apply rounded-md bg-red-400 text-xs text-white tracking-tight px-1.5 leading-6 z-10;
  background: #000 linear-gradient(0deg, #f87171, #f87171);
}
</style>
