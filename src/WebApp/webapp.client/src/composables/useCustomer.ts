import { useCustomerStore } from '@/stores/ordering'
import { computed, watch } from 'vue'
import useIdentity from './useIdentity'

export default function useCustomer() {
  const customerStore = useCustomerStore()
  const { user, isGuest } = useIdentity()

  watch(user, async () => {
    if (isGuest.value) {
      console.log('getCustomer')
    }
  })

  async function refreshOrders() {
    if (isGuest.value) {
      await customerStore.getOrders()
    }
  }

  return {
    customer: computed(() => customerStore.customer),
    order: computed(() => customerStore.order),
    orders: computed(() => customerStore.orders),
    getOrder: customerStore.getOrder,
    isGuest,
    refreshOrders,
    getOrders: customerStore.getOrders,
  }
}
