<script setup lang="ts">
import useBasket from '@/composables/useBasket';
import { useHelpers } from '@/composables/useHelpers';
import type { BasketItem } from '@/types/ordering';
import { computed } from 'vue';

const { updateItemQuantity } = useBasket();
const { FALLBACK_IMG } = useHelpers();

const { item } = defineProps<{
    item: BasketItem
}>();

const productSlug = computed(() => `/products/${decodeURIComponent(item.productId)}`);
const imgScr = computed(() => item.pictureUrl || FALLBACK_IMG);

const removeItem = () => {
    updateItemQuantity(item.productId, 0);
};

</script>

<template>
    <SwipeCard @remove="removeItem">
        <div class="flex items-center gap-3 group">
            <RouterLink :to="productSlug">
                <img width="64" height="64" class="w-16 h-16 rounded-md skeleton" :src="imgScr"
                    :alt="item.productName + ' image'" :title="item.productName" loading="lazy" />
            </RouterLink>
            <div class="flex-1">
                <div class="flex gap-x-2 gap-y-1 flex-wrap items-center">
                    <RouterLink class="leading-tight" :to="productSlug">{{ item.productName }}</RouterLink>
                </div>
                <ProductPrice class="mt-1 text-xs" :sale-price="item.unitPrice" :regular-price="item.unitPrice" />
            </div>
            <div class="inline-flex gap-2 flex-col items-end">
                <QuantityInput :item />
                <div class="text-xs text-gray-400 group-hover:text-gray-700 flex leading-none items-center">
                    <button title="Remove Item" aria-label="Remove Item" @click="removeItem" type="button"
                        class="flex items-center gap-1 hover:text-red-500 cursor-pointer">
                        <UIcon name="ion:trash" class="hidden md:inline-block" size="12" />
                    </button>
                </div>
            </div>
        </div>
    </SwipeCard>
</template>
