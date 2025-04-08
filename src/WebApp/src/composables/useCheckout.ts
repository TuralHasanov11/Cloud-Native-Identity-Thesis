import { ref } from 'vue'
import useBasket from './useBasket'
import useCustomer from './useCustomer'
import useIdentity from './useIdentity'
import type { CardType, CreateOrderRequest } from '@/types/ordering'
import type { BasketCheckoutInfo } from '@/types/basket'
import useOrdering from './useOrdering'

const cardTypes = ref<CardType[]>([])

const isProcessingOrder = ref<boolean>(false)

export default function useCheckout() {
  async function getCardTypes(): Promise<void> {
    const { data } = await useBffFetch('api/ordering/orders/card-types').json<CardType[]>()

    if (data.value) {
      cardTypes.value = data.value
    }
  }

  async function processCheckout(checkoutInfo: BasketCheckoutInfo): Promise<boolean | void> {
    const { login, isGuest } = useIdentity()

    if (isGuest.value) {
      return login()
    }

    const { customer } = useCustomer()
    const { user } = useIdentity()

    const { cart, emptyCart, deleteBasket } = useBasket()
    const { createOrder } = useOrdering()

    isProcessingOrder.value = true

    console.log('processCheckout', checkoutInfo)

    try {
      const orderRequest: CreateOrderRequest = {
        userId: user.value.id,
        userName: user.value.name,
        city: checkoutInfo.city,
        country: checkoutInfo.country,
        state: checkoutInfo.state,
        street: checkoutInfo.street,
        zipCode: checkoutInfo.zipcode,
        customer: customer.value.id,
        items: cart.value.items.map((item) => ({
          productId: item.productId,
          quantity: item.quantity,
          productName: item.productName,
          unitPrice: item.unitPrice,
          oldUnitPrice: item.oldUnitPrice,
        })),
        cardTypeId: checkoutInfo.cardTypeId,
      }

      const orderResult = await createOrder(orderRequest)

      if (orderResult) {
        await emptyCart()
        await deleteBasket()
        return true
      } else {
        alert('There was an error processing your order. Please try again.')
        return false
      }
    } catch (error: unknown) {
      console.error('Error processing order:', error)
      alert('There was an error processing your order. Please try again.')
      return false
    } finally {
      isProcessingOrder.value = false
    }
  }

  return {
    isProcessingOrder,
    processCheckout,
    getCardTypes,
  }
}
