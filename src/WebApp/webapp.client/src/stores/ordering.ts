import { defineStore } from 'pinia'
import { shallowRef, type ShallowRef } from 'vue'
import { OrderStatus, type Customer, type Order, type OrderSummary } from '@/types/ordering'
import useBffFetch from '@/composables/useBffFetch'

export const ORDER_NULL_OBJECT: Order = {
  orderNumber: '',
  date: '',
  description: '',
  city: '',
  country: '',
  state: '',
  street: '',
  zipcode: '',
  status: OrderStatus.Default,
  total: 0,
  orderItems: [],
}

export interface CustomerStore {
  customer: ShallowRef<Customer>
  order: ShallowRef<Order>
  orders: ShallowRef<OrderSummary[]>
  getOrders: () => Promise<void>
  getOrder: (id: string) => Promise<void>
}

export const GUEST_CUSTOMER: Customer = {
  id: '',
  name: '',
  identityId: '',
  paymentMethods: [],
}

export const useCustomerStore = defineStore('customer', (): CustomerStore => {
  const customer = shallowRef<Customer>(GUEST_CUSTOMER)
  const order = shallowRef<Order>(ORDER_NULL_OBJECT)
  const orders = shallowRef<OrderSummary[]>([])

  async function getOrder(id: string): Promise<void> {
    try {
      const { data } = await useBffFetch(`/api/ordering/orders/${id}`).json<Order>()

      if (data.value) {
        order.value = data.value
      }
    } catch (error) {
      console.error('Error fetching order:', error)
      order.value = ORDER_NULL_OBJECT
    }
  }

  async function getOrders(): Promise<void> {
    const { data } = await useBffFetch('/api/ordering/orders/user').json<OrderSummary[]>()

    if (data.value) {
      orders.value = data.value
    }
  }

  return {
    customer,
    order,
    orders,
    getOrder,
    getOrders,
  }
})

// export interface OrdersStore {
// }

// export const useOrdersStore = defineStore('orders', (): OrdersStore => {

// })
