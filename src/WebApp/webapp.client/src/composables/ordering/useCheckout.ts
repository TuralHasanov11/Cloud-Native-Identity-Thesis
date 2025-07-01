import type { BasketCheckoutInfo } from '@/types/basket'
import type { CardType, CreateOrderRequest } from '@/types/ordering'
import { Ok } from 'ts-results'
import { ref } from 'vue'
import useBasket from '../basket/useBasket'
import useIdentity from '../identity/useIdentity'
import useBffFetch from '../useBffFetch'
import useOrders from './useOrders'

const isProcessingOrder = ref<boolean>(false)

export default function useCheckout() {
  function getCardTypes() {
    return useBffFetch('api/ordering/orders/card-types').json<CardType[]>()
  }

  async function processCheckout(checkoutInfo: BasketCheckoutInfo) {
    isProcessingOrder.value = true

    const { user } = useIdentity()

    const { cart, emptyCart } = useBasket()
    const { createOrder } = useOrders()

    const orderRequest: CreateOrderRequest = {
      userId: user.value.id,
      userName: user.value.name,
      city: checkoutInfo.city,
      country: checkoutInfo.country,
      state: checkoutInfo.state,
      street: checkoutInfo.street,
      zipCode: checkoutInfo.zipcode,
      customer: user.value.id,
      items: cart.value.items.map((item) => ({
        productId: item.productId,
        quantity: item.quantity,
        productName: item.productName,
        unitPrice: item.unitPrice,
        oldUnitPrice: item.oldUnitPrice ?? 0,
        pictureUrl: item.pictureUrl ?? '',
      })),
      cardTypeId: checkoutInfo.cardTypeId,
    }

    const orderResult = await createOrder(orderRequest)

    if (orderResult.ok) {
      await emptyCart()
      isProcessingOrder.value = false
      return Ok.EMPTY
    } else {
      isProcessingOrder.value = false
      return orderResult
    }
  }

  return {
    isProcessingOrder,
    processCheckout,
    getCardTypes,
  }
}
