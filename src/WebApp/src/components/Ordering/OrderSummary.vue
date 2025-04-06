<script setup lang="ts">
import useBasket from '@/composables/useBasket';

const { cart, isUpdatingCart, total } = useBasket();
</script>

<template>
    <aside v-if="cart"
        class="bg-white rounded-lg shadow-lg mb-8 w-full min-h-[280px] p-4 sm:p-8 relative md:max-w-md md:top-32 md:sticky">
        <h2 class="mb-6 text-xl font-semibold leading-none">{{ $t('messages.shop.orderSummary') }}</h2>

        <!-- <ul class="flex flex-col gap-4 overflow-y-auto">
            <CartCard v-for="item in cart.items" :key="item.productId" :item />
        </ul> -->

        <div class="grid gap-1 text-sm font-semibold text-gray-500">
            <div class="flex justify-between mt-4">
                <span>{{ $t('messages.shop.total') }}</span>
                <span class="text-lg font-bold text-gray-700 tabular-nums" v-html="total" />
            </div>
        </div>

        <slot></slot>

        <div v-if="isUpdatingCart" class="absolute inset-0 flex items-center justify-center bg-white bg-opacity-50">
            <LoadingIcon />
        </div>
    </aside>
</template>
