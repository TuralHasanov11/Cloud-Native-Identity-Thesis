import { HttpStatusCode } from '@/types/common'
import type { CreateOrderRequest } from '@/types/ordering'
import useBffFetch from '../useBffFetch'
import { Err, Ok } from 'ts-results'

export default function useOrders() {
  async function createOrder(request: CreateOrderRequest) {
    const { statusCode } = await useBffFetch('api/ordering/orders').post(request)

    if (statusCode.value === HttpStatusCode.OK) {
      return Ok.EMPTY
    }

    return new Err('Failed to create order')
  }

  return {
    createOrder,
  }
}
