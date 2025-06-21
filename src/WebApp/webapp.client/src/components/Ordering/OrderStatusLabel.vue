<script setup lang="ts">
import { OrderStatus } from '@/types/ordering'
import { computed } from 'vue'

const props = defineProps<{ status: string }>()

const orderStatus = computed<string>(() => {
  switch (props.status) {
    case OrderStatus.Shipped.toString():
    case OrderStatus.Paid.toString():
      return 'bg-green-50 border-green-100 text-green-600'
    case OrderStatus.Cancelled.toString():
      return 'bg-red-50 border-red-100 text-red-600'
    case OrderStatus.Submitted.toString():
      return 'bg-yellow-50 border-yellow-100 text-yellow-600'
    case OrderStatus.AwaitingValidation.toString():
    case OrderStatus.Default.toString():
      return 'bg-blue-50 border-blue-100 text-blue-600'
    default:
      return 'Unknown Status'
  }
})
</script>

<template>
  <span
    :class="orderStatus"
    class="border rounded-md font-semibold bg-gray-100 text-xs leading-none p-1.5 inline-block"
    >{{ status }}</span
  >
</template>
