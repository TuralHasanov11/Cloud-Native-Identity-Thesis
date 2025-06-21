import { useCustomerStore, GUEST_CUSTOMER } from '@/stores/ordering'
import { computed, onMounted, watch } from 'vue'
import useIdentity from './useIdentity'

export default function useCustomer() {
  const customerStore = useCustomerStore()
  const { user, isGuest: isUserGuest } = useIdentity()

  const isGuest = computed<boolean>(() => customerStore.customer.id === GUEST_CUSTOMER.id)

  watch(user, async () => {
    if (isUserGuest.value) {
      console.log('getCustomer')
    }
  })

  onMounted(async () => {
    if(isUserGuest.value){
      await customerStore.getOrders()
    }
  })

  async function refreshOrders(){
    if (isUserGuest.value) {
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
  }
}
