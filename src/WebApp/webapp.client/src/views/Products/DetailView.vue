<script lang="ts" setup>
import useBasket from '@/composables/basket/useBasket'
import useIdentity from '@/composables/identity/useIdentity'
import { useHelpers } from '@/composables/useHelpers'

import useProducts from '@/composables/catalog/useProducts'
import type { BasketItem } from '@/types/basket'
import { StockStatusEnum } from '@/types/catalog'
import { computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRoute } from 'vue-router'
const { t } = useI18n()
const route = useRoute()
const { addToCart, isUpdatingCart } = useBasket()
const { getProductById } = useProducts()
const { FALLBACK_IMG } = useHelpers()
const { isAuthenticated } = useIdentity()

const { data: product } = await getProductById(route.params.slug as string)

const selectProductInput = computed<BasketItem>(() => ({
  productId: product.value?.id ?? '',
  quantity: quantity.value,
  productName: product.value?.name ?? '',
  unitPrice: product.value?.price ?? 0,
  oldUnitPrice: product.value?.price,
  pictureUrl: product.value?.pictureUrl ?? FALLBACK_IMG,
  id: product.value?.id ?? '',
}))

const quantity = ref<number>(1)

const stockStatus = computed<StockStatusEnum>(() => {
  return product.value != null && product.value.availableStock > 0 ? StockStatusEnum.IN_STOCK : StockStatusEnum.OUT_OF_STOCK
})
const disabledAddToCart = computed(() => {
  return product.value == null || stockStatus.value === StockStatusEnum.OUT_OF_STOCK || isUpdatingCart.value
})
</script>

<template>
  <main id="product">
    <div class="container mx-auto">
      <div v-if="product">
        <div class="flex flex-col gap-10 md:flex-row md:justify-between lg:gap-24">
          <ProductImageGallery v-if="product.pictureUrl" class="relative flex-1" :main-image="product.pictureUrl"
            :gallery="[product.pictureUrl]" :product="product" />
          <img v-else :width="400" :height="400" class="relative flex-1 skeleton" :src="FALLBACK_IMG"
            :alt="product?.name || 'Product'" />

          <div class="lg:max-w-md xl:max-w-lg md:py-2 w-full">
            <div class="flex justify-between mb-4">
              <div class="flex-1">
                <h1 class="flex flex-wrap items-center gap-2 mb-2 text-2xl font-sesmibold">
                  {{ product.name }}
                </h1>
              </div>
              <ProductPrice class="text-xl" :sale-price="product.price" :regular-price="product.price" />
            </div>

            <div class="grid gap-2 my-8 text-sm empty:hidden">
              <div class="flex items-center gap-2">
                <span class="text-gray-400">{{ t('messages.shop.availability') }}: </span>
                <StockStatus :stock-status="stockStatus" />
              </div>
            </div>

            <div class="mb-8 font-light prose">{{ product.description }}</div>

            <hr />

            <template v-if="isAuthenticated">
              <form @submit.prevent="addToCart(selectProductInput)">
                <div
                  class="fixed bottom-0 left-0 z-10 flex items-center w-full gap-4 p-4 mt-12 md:static md:bg-transparent bg-opacity-90 md:p-0">
                  <InputNumber v-model="quantity" type="number" :min="1" aria-label="Quantity" />

                  <AddToCartButton class="flex-1 w-full md:max-w-xs" :disabled="disabledAddToCart" />
                </div>
              </form>
            </template>

            <div v-if="product.categories">
              <div class="grid gap-2 my-8 text-sm">
                <div class="flex items-center gap-2">
                  <span class="text-gray-400">{{ t('messages.shop.category', 2) }}:</span>
                  <div class="product-categories">
                    <RouterLink v-for="category in product.categories" :key="category.id"
                      :to="`/product-category/${decodeURIComponent(category?.slug || '')}`" class="hover:text-primary"
                      :title="category.name">{{ category.name }}<span class="comma">, </span>
                    </RouterLink>
                  </div>
                </div>
              </div>
              <hr />
            </div>
          </div>
        </div>
        <div v-if="product.description" class="my-32">
          <ProductTabs :product />
        </div>
      </div>
    </div>
  </main>
</template>

<style scoped>
.product-categories>a:last-child .comma {
  display: none;
}

input[type='number']::-webkit-inner-spin-button {
  opacity: 1;
}
</style>
