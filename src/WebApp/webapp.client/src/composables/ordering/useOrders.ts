import { HttpStatusCode } from '@/types/common'
import type { CreateOrderRequest } from '@/types/ordering'
import useBffFetch from '../useBffFetch'

export default function useOrders() {
  async function createOrder(request: CreateOrderRequest): Promise<boolean> {
    try {
      const { statusCode, data } = await useBffFetch('api/ordering/orders').post(request)

      if (statusCode.value === HttpStatusCode.OK) {
        return true
      }

      console.error('Error creating order:', data.value)
      return false
    } catch (error) {
      console.error('Error creating order:', error)
      return false
    }
  }

  return {
    createOrder,
  }
}
