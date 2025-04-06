<script setup lang="ts">
import DefaultLayout from '@/layouts/DefaultLayout.vue'

import useBasket from '@/composables/useBasket';
import useCheckout from '@/composables/useCheckout';
import useCustomer from '@/composables/useCustomer';
import useIdentity from '@/composables/useIdentity';
import { computed, onBeforeMount, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRoute } from 'vue-router';

// import { loadStripe } from '@stripe/stripe-js';
// import type { Stripe, StripeElements, CreateSourceData, StripeCardElement } from '@stripe/stripe-js';

const { t } = useI18n();
const { query } = useRoute();
const { cart, isUpdatingCart, isEmpty: cartIsEmpty } = useBasket();
const { isGuest } = useIdentity();
const { customer } = useCustomer()
const { orderInput, isProcessingOrder, processCheckout } = useCheckout();
// const runtimeConfig = useRuntimeConfig();
// const stripeKey = runtimeConfig.public?.STRIPE_PUBLISHABLE_KEY || null;

const buttonText = ref<string>(isProcessingOrder.value ? t('messages.general.processing') : t('messages.shop.checkoutButton'));
const isCheckoutDisabled = computed<boolean>(() => isProcessingOrder.value || isUpdatingCart.value || !orderInput.value.paymentMethod);

// const isInvalidEmail = ref<boolean>(false);
// const stripe: Stripe | null = stripeKey ? await loadStripe(stripeKey) : null;
// const elements = ref();
const isPaid = ref<boolean>(false);

onBeforeMount(async () => {
  if (query.cancel_order) window.close();
});

const payNow = async () => {
  buttonText.value = t('messages.general.processing');

  //   const { stripePaymentIntent } = await GqlGetStripePaymentIntent();
  //   const clientSecret = stripePaymentIntent?.clientSecret || '';

  //   try {
  //     if (orderInput.value.paymentMethod.id === 'stripe' && stripe && elements.value) {
  //       const cardElement = elements.value.getElement('card') as StripeCardElement;
  //       const { setupIntent } = await stripe.confirmCardSetup(clientSecret, { payment_method: { card: cardElement } });
  //       const { source } = await stripe.createSource(cardElement as CreateSourceData);

  //       if (source) orderInput.value.metaData.push({ key: '_stripe_source_id', value: source.id });
  //       if (setupIntent) orderInput.value.metaData.push({ key: '_stripe_intent_id', value: setupIntent.id });

  //       isPaid.value = setupIntent?.status === 'succeeded' || false;
  //       orderInput.value.transactionId = source?.created?.toString() || new Date().getTime().toString();
  //     }
  //   } catch (error) {
  //     console.error(error);
  //     buttonText.value = t('messages.shop.placeOrder');
  //   }

  await processCheckout(isPaid.value);
};

// const handleStripeElement = (stripeElements: StripeElements): void => {
//   elements.value = stripeElements;
// };

</script>

<template>
  <DefaultLayout>
    <main id="checkout">
      <UContainer>
        <div class="flex flex-col min-h-[600px]">
          <template v-if="cart && customer">
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
                  <!-- <div class="w-full mt-4">
              <label for="email">{{ $t('messages.billing.email') }}</label>
              <input v-model="customer.billing.email" placeholder="johndoe@email.com" autocomplete="email" type="email"
                name="email" :class="{ 'has-error': isInvalidEmail }" required
                @blur="checkEmailOnBlur(customer.billing.email)" @input="checkEmailOnInput(customer.billing.email)">
              <Transition name="scale-y" mode="out-in">
                <div v-if="isInvalidEmail" class="mt-1 text-sm text-red-500">Invalid email address</div>
              </Transition>
            </div> -->
                </div>

                <!-- <div>
            <h2 class="w-full mb-3 text-2xl font-semibold">{{ $t('messages.billing.billingDetails') }}</h2>
            <BillingDetails v-model="customer.billing" />
          </div> -->

                <!-- <label v-if="cart.availableShippingMethods.length > 0" for="shipToDifferentAddress"
            class="flex items-center gap-2">
            <span>{{ $t('messages.billing.differentAddress') }}</span>
            <input id="shipToDifferentAddress" v-model="orderInput.shipToDifferentAddress" type="checkbox"
              name="shipToDifferentAddress">
          </label> -->

                <!-- <Transition name="scale-y" mode="out-in">
            <div v-if="orderInput.shipToDifferentAddress">
              <h2 class="mb-4 text-xl font-semibold">{{ $t('messages.general.shippingDetails') }}</h2>
              <ShippingDetails v-model="customer.shipping" />
            </div>
          </Transition> -->

                <!-- Shipping methods -->
                <!-- <div v-if="cart.availableShippingMethods.length">
            <h3 class="mb-4 text-xl font-semibold">{{ $t('messages.general.shippingSelect') }}</h3>
            <ShippingOptions :options="cart.availableShippingMethods[0].rates"
              :active-option="cart.chosenShippingMethods[0]" />
          </div> -->

                <!-- Pay methods -->
                <!-- <div v-if="paymentGateways?.nodes.length" class="mt-2 col-span-full">
            <h2 class="mb-4 text-xl font-semibold">{{ $t('messages.billing.paymentOptions') }}</h2>
            <PaymentOptions v-model="orderInput.paymentMethod" class="mb-4" :payment-gateways />
            <StripeElement v-if="stripe" v-show="orderInput.paymentMethod.id == 'stripe'" :stripe
              @update-element="handleStripeElement" />
          </div> -->

                <!-- Order note -->
                <!-- <div>
            <h2 class="mb-4 text-xl font-semibold">{{ $t('messages.shop.orderNote') }} ({{
              $t('messages.general.optional') }})
            </h2>
            <textarea id="order-note" v-model="orderInput.customerNote" name="order-note" class="w-full min-h-[100px]"
              rows="4" :placeholder="$t('messages.shop.orderNotePlaceholder')" />
          </div> -->
              </div>

              <OrderSummary>
                <button
                  class="flex items-center justify-center w-full gap-3 p-3 mt-4 font-semibold text-center text-white rounded-lg shadow-md bg-primary hover:bg-primary-dark disabled:cursor-not-allowed disabled:bg-gray-400"
                  :disabled="isCheckoutDisabled">
                  {{ buttonText }}
                  <!-- <LoadingIcon v-if="isProcessingOrder" color="#fff" size="18" /> -->
                </button>
              </OrderSummary>
            </form>
          </template>
          <LoadingIcon v-else class="m-auto" />
        </div>
      </UContainer>
    </main>
  </DefaultLayout>
</template>
