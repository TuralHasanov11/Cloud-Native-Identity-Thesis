<script setup lang="ts">
import { formatCurrency } from '@/utils/currency-formatter';

const { regularPrice, salePrice } = defineProps<{
  regularPrice?: number | null
  salePrice?: number | null
}>()
</script>

<template>
  <div v-if="regularPrice" class="flex font-semibold">
    <span :class="{ 'text-gray-400 line-through font-normal': salePrice }">{{ formatCurrency(regularPrice) }}</span>
    <span v-if="salePrice" class="ml-2">{{ salePrice }}</span>
  </div>
</template>
