<script setup lang="ts">
import useBasket from '@/composables/basket/useBasket'
import { useHelpers } from '@/composables/useHelpers'
import { useI18n } from 'vue-i18n'
const { t } = useI18n()
const { cart, total } = useBasket()
const { formatPrice } = useHelpers()
</script>

<template>
  <aside v-if="cart" class="bg-white rounded-lg shadow-lg mb-8 w-full min-h-[280px] p-4 relative">
    <h2 class="mb-6 text-xl font-semibold leading-none">{{ t('messages.shop.orderSummary') }}</h2>

    <ul class="flex flex-col gap-4 overflow-y-auto">
      <CartCard v-for="item in cart.items" :key="item.productId" :item />
    </ul>

    <div class="grid gap-1 text-sm font-semibold text-gray-500">
      <div class="flex justify-between mt-4">
        <span>{{ t('messages.shop.total') }}</span>
        <span class="text-lg font-bold text-gray-700 tabular-nums">{{ formatPrice(total) }}</span>
      </div>
    </div>

    <slot></slot>
  </aside>
</template>
