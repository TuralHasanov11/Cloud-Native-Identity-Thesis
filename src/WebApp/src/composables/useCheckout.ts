import { ref } from 'vue'
import useBasket from './useBasket'
import useCustomer from './useCustomer'
import useIdentity from './useIdentity'
import { useRouter } from 'vue-router'
import { useHelpers } from './useHelpers'
import type { CardType } from '@/types/ordering'
import type { BasketCheckoutInfo } from '@/types/basket'

interface OrderInput {
  customerNote: string
  paymentMethod: string
  metaData?: { key: string; value: string }[]
}

const cardTypes = ref<CardType[]>([])

const isProcessingOrder = ref<boolean>(false)

export default function useCheckout() {
  async function getCardTypes(): Promise<void> {
    const { data } = await useBffFetch('api/ordering/orders/card-types').json<CardType[]>()

    if (data.value) {
      cardTypes.value = data.value
    }
  }

  async function processCheckout(checkoutInfo: BasketCheckoutInfo): Promise<void> {
    const { login, isGuest } = useIdentity()

    if (isGuest.value) {
      return login()
    }

    const { customer } = useCustomer()

    const { cart, emptyCart, deleteBasket } = useBasket()

    isProcessingOrder.value = true

    console.log('processCheckout', checkoutInfo)

    try {
      //   const checkoutPayload: CheckoutInput = {
      //     billing,
      //     shipping,
      //     shippingMethod,
      //     metaData: orderInput.value.metaData,
      //     paymentMethod: orderInput.value.paymentMethod.id,
      //     customerNote: orderInput.value.customerNote,
      //     shipToDifferentAddress,
      //     transactionId: orderInput.value.transactionId,
      //     isPaid,
      //   }
      //   // Create account
      //   if (orderInput.value.createAccount) {
      //     checkoutPayload.account = { username, password } as CreateAccountInput
      //   } else {
      //     // Remove account from checkoutPayload if not creating account otherwise it will create an account anyway
      //     checkoutPayload.account = null
      //   }
      //   const { checkout } = await GqlCheckout(checkoutPayload)
      //   // Login user if account was created during checkout
      //   if (orderInput.value.createAccount) {
      //     await loginUser({ username, password })
      //   }
      //   const orderId = checkout?.order?.databaseId
      //   const orderKey = checkout?.order?.orderKey
      //   const orderInputPaymentId = orderInput.value.paymentMethod.id
      //   const isPayPal = orderInputPaymentId === 'paypal' || orderInputPaymentId === 'ppcp-gateway'
      //   // PayPal redirect
      //   if ((await checkout?.redirect) && isPayPal) {
      //     const frontEndUrl = window.location.origin
      //     let redirectUrl = checkout?.redirect ?? ''
      //     const payPalReturnUrl = `${frontEndUrl}/checkout/order-received/${orderId}/?key=${orderKey}&from_paypal=true`
      //     const payPalCancelUrl = `${frontEndUrl}/checkout/?cancel_order=true&from_paypal=true`
      //     redirectUrl = replaceQueryParam('return', payPalReturnUrl, redirectUrl)
      //     redirectUrl = replaceQueryParam('cancel_return', payPalCancelUrl, redirectUrl)
      //     redirectUrl = replaceQueryParam('bn', 'WooNuxt_Cart', redirectUrl)
      //     const isPayPalWindowClosed = await openPayPalWindow(redirectUrl)
      //     if (isPayPalWindowClosed) {
      //       router.push(`/checkout/order-received/${orderId}/?key=${orderKey}&fetch_delay=true`)
      //     }
      //   } else {
      //     router.push(`/checkout/order-received/${orderId}/?key=${orderKey}`)
      //   }
      //   if ((await checkout?.result) !== 'success') {
      //     alert('There was an error processing your order. Please try again.')
      //     window.location.reload()
      //     return checkout
      //   } else {
      //     await emptyCart()
      //     await deleteBasket()
      //   }
    } catch (error: any) {
      //   const errorMessage = error?.gqlErrors?.[0].message
      //   if (errorMessage?.includes('An account is already registered with your email address')) {
      //     alert('An account is already registered with your email address')
      //     return null
      //   }
      //   alert(errorMessage)
      //   return null
    }

    isProcessingOrder.value = false
  }

  return {
    isProcessingOrder,
    processCheckout,
    getCardTypes,
  }
}
