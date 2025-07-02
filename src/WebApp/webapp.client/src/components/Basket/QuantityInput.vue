<script setup lang="ts">
import useBasket from '@/composables/basket/useBasket';
import type { BasketItem } from '@/types/basket';
import type { InputNumberBlurEvent } from 'primevue';
import { ref } from 'vue';

const { updateItemQuantity, isUpdatingCart } = useBasket()

const { item } = defineProps<{
  item: BasketItem
}>()

const quantity = ref<number>(item.quantity)

async function handleQuantityUpdate(event: InputNumberBlurEvent) {
  await updateItemQuantity(item.productId, Number(event.value));
}

</script>

<template>
  <InputNumber v-model="quantity" :inputId="`minmax-buttons-${item.productId}`" mode="decimal"
    :disabled="isUpdatingCart" showButtons :min="0" :step="1" :max="100" fluid @blur="handleQuantityUpdate" />
</template>
