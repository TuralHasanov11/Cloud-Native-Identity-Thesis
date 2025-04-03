<script setup lang="ts">
import { OrderStatus } from "~/types/ordering";

const { params, name } = useRoute();
const { customer, order, getOrder, isGuest: customerIsGuest } = useCustomer();
const { formatDate, formatPrice } = useHelpers();
const { t } = useI18n();

const isLoaded = ref<boolean>(false);
const errorMessage = ref("");

const isSummaryPage = computed<boolean>(() => name === "order-summary");
const isCheckoutPage = computed<boolean>(() => name === "order-received");
const orderIsNotCompleted = computed<boolean>(
  () =>
    (order.value &&
      order.value?.status !== OrderStatus.AwaitingValidation.toString()) ||
    order.value?.status !== OrderStatus.Cancelled.toString()
);
// const hasDiscount = computed<boolean>(() => !!parseFloat(order.value?.di || '0'));

await callOnce(() => getOrder(params.orderId as string), {
  mode: "navigation",
});

const refreshOrder = async () => {
  isLoaded.value = false;
  await getOrder(params.orderId as string);
};

useSeoMeta({
  title: isSummaryPage.value
    ? t("messages.shop.orderSummary")
    : t("messages.shop.orderReceived"),
});
</script>

<template>
  <main id="order-summary">
    <UContainer>
      <UPageBody>
        <div
          class="w-full min-h-[600px] flex items-center p-4 text-gray-800 md:bg-white md:rounded-xl md:mx-auto md:shadow-lg md:my-24 md:mt-8 md:max-w-3xl md:p-16 flex-col"
        >
          <LoadingIcon v-if="!isLoaded" class="flex-1" />
          <template v-else>
            <div v-if="order" class="w-full">
              <template v-if="isSummaryPage">
                <div class="flex items-center gap-4">
                  <NuxtLink
                    to="/user?tab=orders"
                    class="inline-flex items-center justify-center p-2 border rounded-md"
                    title="Back to orders"
                    aria-label="Back to orders"
                  >
                    <Icon name="ion:chevron-back-outline" />
                  </NuxtLink>
                  <h1 class="text-xl font-semibold">
                    {{ $t("messages.shop.orderSummary") }}
                  </h1>
                </div>
              </template>
              <template v-else-if="isCheckoutPage">
                <div class="flex items-center justify-between w-full mb-2">
                  <h1 class="text-xl font-semibold">
                    {{ $t("messages.shop.orderReceived") }}
                  </h1>
                  <button
                    v-if="orderIsNotCompleted"
                    type="button"
                    class="inline-flex items-center justify-center p-2 bg-white border rounded-md"
                    title="Refresh order"
                    aria-label="Refresh order"
                    @click="refreshOrder"
                  >
                    <Icon name="ion:refresh-outline" />
                  </button>
                </div>
                <p>{{ $t("messages.shop.orderThanks") }}</p>
              </template>
              <hr class="my-8" >
            </div>
            <div v-if="order && !customerIsGuest" class="flex-1 w-full">
              <div class="flex items-start justify-between">
                <div class="w-[21%]">
                  <div class="mb-2 text-xs text-gray-400 uppercase">
                    {{ $t("messages.shop.order") }}
                  </div>
                  <div class="leading-none">#{{ order.orderNumber! }}</div>
                </div>
                <div class="w-[21%]">
                  <div class="mb-2 text-xs text-gray-400 uppercase">
                    {{ $t("messages.general.date") }}
                  </div>
                  <div class="leading-none">{{ formatDate(order.date) }}</div>
                </div>
                <div class="w-[21%]">
                  <div class="mb-2 text-xs text-gray-400 uppercase">
                    {{ $t("messages.general.status") }}
                  </div>
                  <OrderStatusLabel :order="order" />
                </div>
                <div class="w-[21%]">
                  <div class="mb-2 text-xs text-gray-400 uppercase">
                    {{ $t("messages.general.paymentMethod") }}
                  </div>
                  <!-- <div class="leading-none">{{ order.paymentMethodTitle }}</div> -->
                </div>
              </div>

              <template v-if="order.orderItems">
                <hr class="my-8" >

                <div class="grid gap-2">
                  <div
                    v-for="item in order.orderItems"
                    :key="item.productId"
                    class="flex items-center justify-between gap-8"
                  >
                    <NuxtLink :to="`/products/${item.productId}`">
                      <NuxtImg
                        class="w-16 h-16 rounded-xl"
                        :src="item.pictureUrl || '/images/placeholder.png'"
                        :alt="'Product image'"
                        :title="'Product image'"
                        width="64"
                        height="64"
                        loading="lazy"
                      />
                    </NuxtLink>

                    <div class="text-sm text-gray-600">
                      Qty. {{ item.units }}
                    </div>
                    <span class="text-sm font-semibold">{{
                      formatPrice(item.totalPrice)
                    }}</span>
                  </div>
                </div>
              </template>

              <hr class="my-8" >

              <div>
                <!-- <div class="flex justify-between">
            <span>{{ $t("messages.shop.subtotal") }}</span>
            <span>{{ order.subtotal }}</span>
          </div>
          <div class="flex justify-between">
            <span>{{ $t("messages.general.tax") }}</span>
            <span>{{ order.totalTax }}</span>
          </div>
          <div class="flex justify-between">
            <span>{{ $t("messages.general.shipping") }}</span>
            <span>{{ order.shippingTotal }}</span>
          </div>
          <div v-if="hasDiscount" class="flex justify-between text-primary">
            <span>{{ $t("messages.shop.discount") }}</span>
            <span>- {{ order.discountTotal }}</span>
          </div> -->
                <hr class="my-8" >
                <div class="flex justify-between">
                  <span class>{{ $t("messages.shop.total") }}</span>
                  <span class="font-semibold">{{ order.total }}</span>
                </div>
              </div>
            </div>
            <div
              v-else-if="errorMessage"
              class="flex flex-col items-center justify-center flex-1 w-full gap-4 text-center"
            >
              <Icon name="ion:sad-outline" size="96" class="text-gray-700" />
              <h1 class="text-xl font-semibold">Error</h1>
              <div v-if="errorMessage" class="text-sm text-red-500">
                {{ errorMessage }}
              </div>
            </div>
          </template>
        </div>
      </UPageBody>
    </UContainer>
  </main>
</template>
