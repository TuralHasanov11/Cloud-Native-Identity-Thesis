import { useCustomerStore } from '@/stores/ordering'
import { type Order, type OrderSummary } from '@/types/ordering'
import { computed, watch } from 'vue'
import useIdentity from '../identity/useIdentity'
import useBffFetch from '../useBffFetch'

export default function useCustomer() {
  const { isGuest } = useIdentity()
  const customerStore = useCustomerStore()

  watch(isGuest, async () => {
    if (isGuest.value) {
      console.log('getCustomer')
    }
  })

  return {
    customer: computed(() => customerStore.customer),
    getOrders() {
      return useBffFetch('/api/ordering/orders/user').json<OrderSummary[]>()
    },
    getOrder(id: string) {
      return useBffFetch(`/api/ordering/orders/${id}`).json<Order>()
    },
  }
}
