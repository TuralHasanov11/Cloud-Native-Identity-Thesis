<script setup lang="ts">
import useCustomer from '@/composables/ordering/useCustomer'
import { useHelpers } from '@/composables/useHelpers'
import { OrderStatus } from '@/types/ordering'
import { computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRoute } from 'vue-router'
const { t } = useI18n()
const { params, name } = useRoute()
const { getOrder } = useCustomer()
const { formatDate, formatPrice, FALLBACK_IMG } = useHelpers()

const { data: order, execute: refreshOrder } = await getOrder(params.id as string)

const errorMessage = ref('')

const isSummaryPage = computed<boolean>(() => name === 'order-summary')
const isCheckoutPage = computed<boolean>(() => name === 'order-received')
const orderIsNotCompleted = computed<boolean>(
  () =>
    (order.value && order.value?.status !== OrderStatus.AwaitingValidation.toString()) || order.value?.status !== OrderStatus.Cancelled.toString(),
)
</script>

<template>
  <main id="order-summary">
    <div class="container mx-auto">
      <div
        class="w-full min-h-[600px] flex items-center p-4 text-gray-800 md:bg-white md:rounded-xl md:mx-auto md:shadow-lg md:my-24 md:mt-8 md:max-w-3xl md:p-16 flex-col"
      >
        <LoadingIcon v-if="!order" class="flex-1" />
        <template v-else>
          <div v-if="order" class="w-full">
            <template v-if="isSummaryPage">
              <div class="flex items-center gap-4">
                <RouterLink
                  to="/user?tab=orders"
                  class="inline-flex items-center justify-center p-2 border rounded-md"
                  title="Back to orders"
                  aria-label="Back to orders"
                >
                  <i class="pi pi-chevron-circle-left" />
                </RouterLink>
                <h1 class="text-xl font-semibold">
                  {{ t('messages.shop.orderSummary') }}
                </h1>
              </div>
            </template>
            <template v-else-if="isCheckoutPage">
              <div class="flex items-center justify-between w-full mb-2">
                <h1 class="text-xl font-semibold">
                  {{ t('messages.shop.orderReceived') }}
                </h1>
                <Button
                  v-if="orderIsNotCompleted"
                  type="button"
                  class="inline-flex items-center justify-center p-2 bg-white border rounded-md"
                  title="Refresh order"
                  aria-label="Refresh order"
                  @click="async () => await refreshOrder()"
                >
                  <i class="pi pi-refresh" />
                </Button>
              </div>
              <p>{{ t('messages.shop.orderThanks') }}</p>
            </template>
            <hr class="my-8" />
          </div>
          <div v-if="order" class="flex-1 w-full">
            <div class="flex items-start justify-between">
              <div class="w-[21%]">
                <div class="mb-2 text-xs text-gray-400 uppercase">
                  {{ t('messages.shop.order') }}
                </div>
                <div class="leading-none">#{{ order.orderNumber! }}</div>
              </div>
              <div class="w-[21%]">
                <div class="mb-2 text-xs text-gray-400 uppercase">
                  {{ t('messages.general.date') }}
                </div>
                <div class="leading-none">{{ formatDate(order.date) }}</div>
              </div>
              <div class="w-[21%]">
                <div class="mb-2 text-xs text-gray-400 uppercase">
                  {{ t('messages.general.status') }}
                </div>
                <OrderStatusLabel :status="order.status" />
              </div>
              <div class="w-[21%]">
                <div class="mb-2 text-xs text-gray-400 uppercase">
                  {{ t('messages.general.paymentMethod') }}
                </div>
              </div>
            </div>

            <template v-if="order.orderItems">
              <hr class="my-8" />

              <div class="grid gap-2">
                <div v-for="item in order.orderItems" :key="item.productId" class="flex items-center justify-between gap-8">
                  <RouterLink :to="`/products/${item.productId}`">
                    <img
                      class="w-16 h-16 rounded-xl"
                      :src="item.pictureUrl || FALLBACK_IMG"
                      :alt="'Product'"
                      :title="'Product image'"
                      width="64"
                      height="64"
                      loading="lazy"
                    />
                  </RouterLink>

                  <div class="text-sm text-gray-600">Qty. {{ item.units }}</div>
                  <span class="text-sm font-semibold">{{ formatPrice(item.totalPrice) }}</span>
                </div>
              </div>
            </template>

            <hr class="my-8" />

            <div>
              <hr class="my-8" />
              <div class="flex justify-between">
                <span class>{{ t('messages.shop.total') }}</span>
                <span class="font-semibold">{{ order.total }}</span>
              </div>
            </div>
          </div>
          <div v-else-if="errorMessage" class="flex flex-col items-center justify-center flex-1 w-full gap-4 text-center">
            <i class="pi pi-times-circle text-gray-700" />
            <h1 class="text-xl font-semibold">Error</h1>
            <div v-if="errorMessage" class="text-sm text-red-500">
              {{ errorMessage }}
            </div>
          </div>
        </template>
      </div>
    </div>
  </main>
</template>
