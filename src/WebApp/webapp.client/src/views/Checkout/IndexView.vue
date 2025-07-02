<script setup lang="ts">
import useBasket from '@/composables/basket/useBasket'
import useCheckout from '@/composables/ordering/useCheckout'
import useIdentity from '@/composables/identity/useIdentity'

import { computed, onBeforeMount, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRoute, useRouter } from 'vue-router'
import { useToast } from 'primevue'

const { t } = useI18n()
const { query } = useRoute()
const router = useRouter()
const { cart, isUpdatingCart, isEmpty: cartIsEmpty } = useBasket()
const { user } = useIdentity()
const toast = useToast()
const { isProcessingOrder, processCheckout, getCardTypes } = useCheckout()

const { data: cardTypes } = await getCardTypes()
console.log(cardTypes)

const buttonText = ref<string>(isProcessingOrder.value ? t('messages.general.processing') : t('messages.shop.checkoutButton'))
const isCheckoutDisabled = computed<boolean>(() => isProcessingOrder.value || isUpdatingCart.value)

onBeforeMount(async () => {
  if (query.cancel_order) window.close()
})

const payNow = async (event: Event) => {
  buttonText.value = t('messages.general.processing')

  if (user.value.address) {
    const result = await processCheckout({
      street: user.value.address.street,
      city: user.value.address.city,
      state: user.value.address.state,
      country: user.value.address.country,
      zipcode: user.value.address.zipCode,
      cardTypeId: 1, // TODO: get from API,
    });
    if (result.ok) {
      toast.add({
        severity: 'success',
        summary: t('messages.shop.order'),
        detail: t('messages.shop.orderReceived'),
        life: 3000
      })
      router.push({
        name: 'user-orders',
      })
    } else {
      toast.add({
        severity: 'error',
        summary: t('messages.shop.orderNotReceived'),
        detail: result.val,
        life: 3000
      })
    }
  }
}
</script>

<template>
  <main id="checkout" class="py-5">
    <div class="flex flex-col min-h-[600px]">
      <template v-if="cart">
        <div v-if="cartIsEmpty" class="flex flex-col items-center justify-center flex-1 mb-12">
          <i class="pi pi-shopping-cart opacity-25 mb-5" />
          <h2 class="text-2xl font-bold mb-2">{{ t('messages.shop.cartEmpty') }}</h2>
          <span class="text-gray-400 mb-4">{{ t('messages.shop.addProductsInYourCart') }}</span>
          <RouterLink to="/products"
            class="flex items-center justify-center gap-3 p-2 px-3 mt-4 font-semibold text-center text-white rounded-lg shadow-md bg-primary hover:bg-primary-dark">
            {{ t('messages.shop.browseOurProducts') }}
          </RouterLink>
        </div>

        <form v-else class="flex flex-wrap items-start gap-8 my-16 justify-evenly lg:gap-20" @submit.prevent="payNow">
          <div class="grid w-full gap-8 checkout-form md:flex-1">
            <!-- Customer details -->
            <div>
              <h2 class="w-full mb-3 font-semibold">
                {{ t('messages.billing.billingDetails') }}
              </h2>
              <BillingDetails v-if="user.address" v-model:address="user.address" />
            </div>
          </div>

          <OrderSummary>
            <Button type="submit" color="primary" :disabled="isCheckoutDisabled">
              {{ buttonText }}
            </Button>
          </OrderSummary>
        </form>
      </template>
      <LoadingIcon v-else class="m-auto" />
    </div>
  </main>
</template>
