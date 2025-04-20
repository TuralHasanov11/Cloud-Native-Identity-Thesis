<script setup lang="ts">
import useBasket from '@/composables/useBasket';
import { useHelpers } from '@/composables/useHelpers';

const { cart, toggleCart, isUpdatingCart, isEmpty, productCount, total } = useBasket();

const { formatPrice } = useHelpers();

</script>

<template>
    <div class="fixed top-0 bottom-0 right-0 z-50 flex flex-col w-11/12 max-w-lg overflow-x-hidden bg-white shadow-lg">
        <UButton icon="ion:close-outline"
            class="absolute p-1 rounded-lg shadow-lg top-6 left-6 md:left-8 cursor-pointer"
            @click="toggleCart(false)" />

        <EmptyCart v-if="cart && !isEmpty" class="rounded-lg shadow-lg p-1.5 hover:bg-red-400 hover:text-white" />

        <div class="mt-8 text-center">
            {{ $t('messages.shop.cart') }}
            <span v-if="productCount"> ({{ productCount }}) </span>
        </div>

        <template v-if="cart && !isEmpty">
            <ul class="flex flex-col flex-1 gap-4 p-6 overflow-y-scroll md:p-8">
                <CartCard v-for="item in cart.items" :key="item.productId" :item />
            </ul>
            <div class="px-8 mb-8">

                <RouterLink
                    class="block p-3 text-lg text-center text-white bg-gray-800 rounded-lg shadow-md justify-evenly hover:bg-gray-900"
                    to="/checkout" @click.prevent="toggleCart()">
                    <span class="mx-2">{{ $t('messages.shop.checkout') }}</span>
                    <span>{{ formatPrice(total) }}</span>

                </RouterLink>
            </div>
        </template>
        <!-- Empty Cart Message -->
        <EmptyCartMessage v-else-if="cart && isEmpty" />
        <!-- Cart Loading -->
        <div v-else class="flex flex-col items-center justify-center flex-1 mb-20">
            <LoadingIcon />
        </div>
        <!-- Cart Loading Overlay -->
        <div v-if="isUpdatingCart" class="absolute inset-0 flex items-center justify-center bg-white bg-opacity-25">
            <LoadingIcon />
        </div>
    </div>
</template>
