import useBffFetch from '@/composables/useBffFetch'
import { OrderStatus, type Order } from '@/types/ordering'
import { ref } from 'vue'

export default function useAdminOrders() {
  const orders = ref<Order[]>([])

  async function getOrders(): Promise<void> {
    const { data } = await useBffFetch('/api/ordering/orders').json<Order[]>()
    if (data.value) {
      orders.value = data.value
    }
  }

  function isSubmitted(order: Order): boolean {
    return order.status === OrderStatus.Submitted
  }

  function isAwaitingValidation(order: Order): boolean {
    return order.status === OrderStatus.AwaitingValidation
  }

  function isStockConfirmed(order: Order): boolean {
    return order.status === OrderStatus.StockConfirmed
  }

  function isPaid(order: Order): boolean {
    return order.status === OrderStatus.Paid
  }

  function isShipped(order: Order): boolean {
    return order.status === OrderStatus.Shipped
  }

  function isCancelled(order: Order): boolean {
    return order.status === OrderStatus.Cancelled
  }

  return {
    orders,
    getOrders,
    isSubmitted,
    isAwaitingValidation,
    isStockConfirmed,
    isPaid,
    isShipped,
    isCancelled,
  }
}
