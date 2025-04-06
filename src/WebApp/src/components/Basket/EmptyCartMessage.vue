<script setup lang="ts">
import useBasket from '@/composables/useBasket';

const { toggleCart } = useBasket();
</script>

<template>
  <div class="flex flex-col items-center justify-center flex-1 mb-20 text-gray-400">
    <UIcon name="ion:cart-outline" size="96" class="opacity-75 mb-5" />
    <div class="mb-2 text-xl font-semibold">{{ $t('messages.shop.cartEmpty') }}</div>
    <span class="mb-8">{{ $t('messages.shop.addProductsInYourCart') }}</span>
    <RouterLink :to="{ name: 'products' }"
      class="flex items-center justify-center gap-3 p-2 px-3 mt-4 font-semibold text-center text-white rounded-lg shadow-md bg-primary hover:bg-primary-dark"
      @click="toggleCart(false)">
      {{ $t('messages.shop.browseOurProducts') }}
    </RouterLink>
  </div>
</template>
