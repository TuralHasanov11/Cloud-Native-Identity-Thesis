<script lang="ts" setup>
import useBasket from '@/composables/basket/useBasket'
import { useI18n } from 'vue-i18n'
const { t } = useI18n()
const { cart, isEmpty, productCount, total } = useBasket()
</script>

<template>
  <main id="cart" class="py-5 relative">
    <div class="container mx-auto">
      <template v-if="!isEmpty">
        <EmptyCart v-if="!isEmpty" class="rounded-lg shadow-lg p-1.5 hover:bg-red-400 hover:text-white" />
        <div class="mt-8 text-center">
          {{ t('messages.shop.cart') }}
          <span v-if="productCount"> ({{ productCount }}) </span>
        </div>

        <ul class="flex flex-col flex-1 gap-4 p-6 overflow-y-scroll md:p-8">
          <CartCard v-for="item in cart.items" :key="item.productId" :item />
        </ul>
        <div class="px-8 mb-8">
          <RouterLink
            class="block p-3 text-lg text-center text-white bg-gray-800 rounded-lg shadow-md justify-evenly hover:bg-gray-900"
            to="/checkout"
          >
            <span class="mx-2">{{ t('messages.shop.checkout') }}</span>
            <span>{{ total }}</span>
          </RouterLink>
        </div>
      </template>
      <EmptyCartMessage v-else />
    </div>
  </main>
</template>
