<script setup lang="ts">
import { StockStatusEnum } from '@/types/catalog';

defineProps<{
  stockStatus: StockStatusEnum
}>();

</script>

<template>
  <span v-if="stockStatus === StockStatusEnum.IN_STOCK" class="text-green-600">{{ $t('messages.shop.inStock') }}</span>
  <span v-else-if="stockStatus === StockStatusEnum.OUT_OF_STOCK" class="text-red-600">{{ $t('messages.shop.outOfStock')
    }}</span>
  <span v-else class="text-gray-600">Loading</span>
</template>
