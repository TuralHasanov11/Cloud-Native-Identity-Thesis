import { GUEST_CUSTOMER, type Customer } from '@/types/ordering'
import { defineStore } from 'pinia'
import { shallowRef, type ShallowRef } from 'vue'

export interface CustomerStore {
  customer: ShallowRef<Customer>
}

export const useCustomerStore = defineStore('customer', (): CustomerStore => {
  const customer = shallowRef<Customer>(GUEST_CUSTOMER)
  return {
    customer,
  }
})
