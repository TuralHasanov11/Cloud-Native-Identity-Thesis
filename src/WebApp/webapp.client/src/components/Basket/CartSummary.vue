<script setup lang="ts">
import useBasket from '@/composables/basket/useBasket'
import { useHelpers } from '@/composables/useHelpers'
import { useI18n } from 'vue-i18n'
import EmptyCart from './EmptyCart.vue'

const visible = defineModel<boolean>('visible', {
  default: false
})

const { t } = useI18n()

const { cart, toggleCart, isEmpty, productCount, total } = useBasket()

const { formatPrice } = useHelpers()
</script>

<template>
  <Dialog v-model:visible="visible" modal :style="{ width: '50rem' }" position="topright">
    <template #header>
      <div class="inline-flex items-center justify-center gap-2">
        <Avatar icon="pi pi-shopping-cart" shape="circle" />
        <span class="font-bold whitespace-nowrap">Cart Summary</span>
      </div>
    </template>

    <EmptyCart v-if="cart && !isEmpty" class="rounded-lg shadow-lg p-1.5 hover:bg-red-400 hover:text-white" />

    <div class="mt-8 text-center">
      {{ t('messages.shop.cart') }}
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
          <span class="mx-2">{{ t('messages.shop.checkout') }}</span>
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

    <template #footer>
      <Button label="Close" text severity="secondary" @click="visible = false" autofocus />
    </template>
  </Dialog>


</template>
