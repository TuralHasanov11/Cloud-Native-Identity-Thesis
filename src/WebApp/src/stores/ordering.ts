import { defineStore } from 'pinia'
import { shallowRef, type ShallowRef } from 'vue'
import type { Customer, Order } from '@/types/ordering'

export const ORDER_NULL_OBJECT: Order = {
  orderNumber: '',
  date: '',
  description: '',
  city: '',
  country: '',
  state: '',
  street: '',
  zipcode: '',
  status: '',
  total: 0,
  orderItems: [],
}

export interface CustomerStore {
  customer: ShallowRef<Customer>
  order: ShallowRef<Order>
  orders: ShallowRef<Order[]>
  getOrders: () => Promise<void>
  getOrder: (id: string) => Promise<void>
}

export const GUEST_CUSTOMER: Customer = {
  id: '',
  name: '',
  identityId: '',
  paymentMethods: [],
  address: {
    city: '',
    country: '',
    state: '',
    street: '',
    zipcode: '',
  },
}

export const useCustomerStore = defineStore('customer', (): CustomerStore => {
  const customer = shallowRef<Customer>(GUEST_CUSTOMER)
  const order = shallowRef<Order>(ORDER_NULL_OBJECT)
  const orders = shallowRef<Order[]>([])

  async function getOrder(id: string): Promise<void> {
    console.log(id)
  }

  async function getOrders(): Promise<void> {}

  return {
    customer,
    order,
    orders,
    getOrder,
    getOrders,
  }
})

// export interface OrdersStore {}

// export const useOrdersStore = defineStore("orders", (): OrdersStore => {
//   return {};
// });
