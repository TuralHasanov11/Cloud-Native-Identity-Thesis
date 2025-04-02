<script setup lang="ts">
interface ProductPriceProps {
  regularPrice?: number | null;
  salePrice?: number | null;
}

const { regularPrice, salePrice } = defineProps<ProductPriceProps>();
</script>

<template>
  <div v-if="regularPrice" class="flex font-semibold">
    <span :class="{ 'text-gray-400 line-through font-normal': salePrice }" >{{regularPrice}}</span>
    <span v-if="salePrice" class="ml-2" >{{salePrice}}</span>
  </div>
</template>
