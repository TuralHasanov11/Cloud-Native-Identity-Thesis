<script setup lang="ts">
import DefaultLayout from '@/layouts/DefaultLayout.vue'

import { computed, onBeforeMount, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRoute, useRouter } from 'vue-router';

const { t } = useI18n();
const { query } = useRoute();
const router = useRouter();
const { cart, isUpdatingCart, isEmpty: cartIsEmpty } = useBasket();
const { isGuest } = useIdentity();
const { user } = useIdentity()
const { isProcessingOrder, processCheckout, getCardTypes } = useCheckout();

const buttonText = ref<string>(isProcessingOrder.value ? t('messages.general.processing') : t('messages.shop.checkoutButton'));
const isCheckoutDisabled = computed<boolean>(() => isProcessingOrder.value || isUpdatingCart.value);

await getCardTypes()

onBeforeMount(async () => {
  if (query.cancel_order) window.close();
});

const payNow = async () => {
  buttonText.value = t('messages.general.processing');

  if (user.value.address) {
    if (await processCheckout({
      street: user.value.address.street,
      city: user.value.address.city,
      state: user.value.address.state,
      country: user.value.address.country,
      zipcode: user.value.address.zipCode,
      cardTypeId: 1, // TODO: get from API,
    })) {
      router.replace({
        name: 'user-orders'
      });
    }
  }


};


</script>

<template>
  <DefaultLayout>
    <main id="checkout" class="py-5">
      <UContainer>
        <div class="flex flex-col min-h-[600px]">
          <template v-if="cart">
            <div v-if="cartIsEmpty" class="flex flex-col items-center justify-center flex-1 mb-12">
              <UIcon name="ion:cart-outline" size="156" class="opacity-25 mb-5" />
              <h2 class="text-2xl font-bold mb-2">{{ $t('messages.shop.cartEmpty') }}</h2>
              <span class="text-gray-400 mb-4">{{ $t('messages.shop.addProductsInYourCart') }}</span>
              <RouterLink to="/products"
                class="flex items-center justify-center gap-3 p-2 px-3 mt-4 font-semibold text-center text-white rounded-lg shadow-md bg-primary hover:bg-primary-dark">
                {{ $t('messages.shop.browseOurProducts') }}
              </RouterLink>
            </div>

            <form v-else class="container flex flex-wrap items-start gap-8 my-16 justify-evenly lg:gap-20"
              @submit.prevent="payNow">
              <div class="grid w-full max-w-2xl gap-8 checkout-form md:flex-1">
                <!-- Customer details -->
                <div v-if="isGuest">
                  <h2 class="w-full mb-2 text-2xl font-semibold leading-none">Contact Information</h2>
                  <p class="mt-1 text-sm text-gray-500">Already have an account? <a href="/my-account"
                      class="text-primary text-semibold">Log in</a>.</p>
                </div>

                <div>
                  <h2 class="w-full mb-3 text-2xl font-semibold">{{ $t('messages.billing.billingDetails') }}</h2>
                  <BillingDetails v-if="user.address" v-model:address="user.address" />
                </div>

              </div>

              <OrderSummary>
                <UButton type="submit" :disabled="isCheckoutDisabled">
                  {{ buttonText }}
                </UButton>
              </OrderSummary>
            </form>
          </template>
          <LoadingIcon v-else class="m-auto" />
        </div>
      </UContainer>
    </main>
  </DefaultLayout>
</template>
