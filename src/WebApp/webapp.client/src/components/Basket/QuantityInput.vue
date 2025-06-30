<script setup lang="ts">
import useBasket from '@/composables/basket/useBasket'
import type { BasketItem } from '@/types/basket'
import { ref, watch } from 'vue'

const { updateItemQuantity, isUpdatingCart } = useBasket()

const { item } = defineProps<{
  item: BasketItem
}>()

const quantity = ref<number>(item.quantity)

watch(quantity, async (newQuantity) => {
  if (newQuantity >= 0) {
    await updateItemQuantity(item.productId, newQuantity)
  }
})
</script>

<template>
  <InputNumber v-model="quantity" :inputId="`minmax-buttons-${item.productId}`" mode="decimal"
    :disabled="isUpdatingCart" showButtons :min="0" :step="1" :max="100" fluid />
</template>
