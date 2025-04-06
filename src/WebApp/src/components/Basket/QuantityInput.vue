<script setup lang="ts">
import useBasket from '@/composables/useBasket';
import { useDebounceFn } from '@vueuse/core';
import { computed, ref, watch } from 'vue';

const { updateItemQuantity, isUpdatingCart, cart } = useBasket();

const { item } = defineProps({ item: { type: Object, required: true } });

const productType = computed(() => (item.variation ? item.variation?.node : item.product?.node));
const quantity = ref<number>(item.quantity)
const hasNoMoreStock = computed(() => (productType.value.stockQuantity ? productType.value.stockQuantity <= quantity.value : false));

const incrementQuantity = useDebounceFn(() => {
    quantity.value++;
}, 1000)
const decrementQuantity = useDebounceFn(() => {
    quantity.value--;
}, 1000)

watch(
    quantity,
    (newQuantity) => {
        if (newQuantity >= 0) {
            updateItemQuantity(item.key, newQuantity);
        }
    },
);

const onFocusOut = () => {
    if (quantity.value <= 0) {
        const cartItem = cart.value.items.find(node => node.productId === item.productId);

        if (cartItem) {
            quantity.value = cartItem.quantity;
        }
    }
};
</script>

<template>
    <div class="flex rounded bg-white text-sm leading-none shadow-sm shadow-gray-200 isolate">
        <button title="Decrease Quantity" aria-label="Decrease Quantity" @click="decrementQuantity" type="button"
            class="focus:outline-none border-r w-6 h-6 border rounded-l border-gray-300 hover:bg-gray-50 disabled:cursor-not-allowed"
            :disabled="isUpdatingCart || quantity <= 0">
            <UIcon name="ion:remove" size="14" />
        </button>
        <input v-model.number="quantity" type="number" min="0" :max="productType.stockQuantity" aria-label="Quantity"
            @focusout="onFocusOut"
            class="flex items-center justify-center w-8 px-2 text-right text-xs focus:outline-none border-y border-gray-300" />
        <button title="Increase Quantity" aria-label="Increase Quantity" @click="incrementQuantity" type="button"
            class="focus:outline-none border-l w-6 h-6 border rounded-r hover:bg-gray-50 border-gray-300 disabled:cursor-not-allowed disabled:bg-gray-100"
            :disabled="isUpdatingCart || hasNoMoreStock">
            <UIcon name="ion:add" size="14" />
        </button>
    </div>
</template>

<style scoped lang="postcss">
input[type='number']::-webkit-inner-spin-button,
input[type='number']::-webkit-outer-spin-button {
    -webkit-appearance: none;
    margin: 0;
}

input[type='number'] {
    -moz-appearance: textfield;
    appearance: textfield;
}
</style>
