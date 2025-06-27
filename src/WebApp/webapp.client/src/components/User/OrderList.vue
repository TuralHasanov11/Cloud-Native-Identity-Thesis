<script setup lang="ts">
import useCustomer from '@/composables/useCustomer'
import { useHelpers } from '@/composables/useHelpers'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { onMounted } from 'vue'
const { t } = useI18n()
const router = useRouter()
const { formatDate, scrollToTop } = useHelpers()
const { orders, refreshOrders, getOrders } = useCustomer()

const goToOrder = (orderNumber: string): void => {
  router.push(`/user/orders/${orderNumber}`)
}

onMounted(async () => {
  await getOrders()
})

</script>

<template>
  <div class="bg-white rounded-lg flex shadow min-h-[250px] p-4 md:p-12 justify-center items-center">
    <div v-if="orders && orders.length" class="w-full">
      <table class="w-full text-left table-auto" aria-label="Order List">
        <thead>
          <tr>
            <th>{{ t('messages.shop.order') }}</th>
            <th>{{ t('messages.general.date') }}</th>
            <th>{{ t('messages.general.status') }}</th>
            <th class="text-right">{{ t('messages.shop.total') }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="order in orders" :key="order.orderNumber"
            class="cursor-pointer hover:underline text-sm text-gray-500 hover:text-gray-800"
            @click="goToOrder(order.orderNumber)">
            <td class="rounded-l-lg">{{ order.orderNumber }}</td>
            <td>{{ formatDate(order.date) }}</td>
            <td>
              <OrderStatusLabel v-if="order.status" :status="order.status" />
            </td>
            <td class="text-right rounded-r-lg">{{ order.total }}</td>
          </tr>
        </tbody>
      </table>
      <div class="text-center flex justify-center w-full mt-8">
        <Button type="button" @click="async () => {
          scrollToTop()
          await refreshOrders()
        }">
          <span>Reresh list</span>
          <i class="pi pi-refresh" />
        </Button>
      </div>
    </div>
    <div v-else-if="orders && orders.length === 0"
      class="min-h-[250px] flex items-center justify-center text-gray-500 text-lg">
      No orders found.
    </div>
    <LoadingIcon v-else size="24" stroke="2" />
  </div>
</template>

<style lang="css" scoped>
tbody tr:nth-child(odd) {
  background-color: #fafafa;
}
</style>
