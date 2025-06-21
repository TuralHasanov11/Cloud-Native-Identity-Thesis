<script setup lang="ts">
import useBasket from '@/composables/useBasket'
import type { BasketItem } from '@/types/basket'
import { ref, watch } from 'vue'

const { updateItemQuantity, isUpdatingCart, cart } = useBasket()

const { item } = defineProps<{
  item: BasketItem
}>()

const quantity = ref<number>(item.quantity)

watch(quantity, (newQuantity) => {
  if (newQuantity >= 0) {
    updateItemQuantity(item.productId, newQuantity)
  }
})

const onFocusOut = () => {
  if (quantity.value <= 0) {
    const cartItem = cart.value.items.find((node) => node.productId === item.productId)

    if (cartItem) {
      quantity.value = cartItem.quantity
    }
  }
}
</script>

<template>
  <InputNumber
    v-model="quantity"
    showButtons
    buttonLayout="horizontal"
    :step="1"
    :min="0"
    :disabled="isUpdatingCart"
    @blur="onFocusOut"
  >
    <template #incrementbutton>
      <span class="pi pi-plus" />
    </template>
    <template #decrementbutton>
      <span class="pi pi-minus" />
    </template>
  </InputNumber>
</template>
