import { useCustomerStore, GUEST_CUSTOMER } from '@/stores/ordering'
import { computed, watch } from 'vue'
import useIdentity from './useIdentity'

export default function useCustomer() {
  const customerStore = useCustomerStore()
  const { user, isGuest: isUserGuest } = useIdentity()

  const isGuest = computed<boolean>(() => customerStore.customer.id === GUEST_CUSTOMER.id)

  async function getCustomer(): Promise<void> {
    console.log('getCustomer')
  }

  watch(user, async () => {
    if (isUserGuest) {
      await getCustomer()
    }
  })

  return {
    customer: computed(() => customerStore.customer),
    order: computed(() => customerStore.order),
    orders: computed(() => customerStore.orders),
    getOrder: customerStore.getOrder,
    getOrders: customerStore.getOrders,
    isGuest,
  }
}
