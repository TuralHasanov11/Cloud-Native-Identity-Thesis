import type { CreateOrderRequest } from '@/types/ordering'

export default function useOrdering() {
  async function createOrder(request: CreateOrderRequest): Promise<boolean> {
    try {
      await useBffFetch('api/ordering/orders').post(request)

      return true
    } catch (error) {
      console.error('Error creating order:', error)
      return false
    }
  }

  return {
    createOrder,
  }
}
