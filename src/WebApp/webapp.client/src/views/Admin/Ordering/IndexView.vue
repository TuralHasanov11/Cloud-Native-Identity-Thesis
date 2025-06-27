<script setup lang="ts">
import useAdminOrders from '@/composables/admin/ordering/useAdminOrders';
import type { Order } from '@/types/ordering';
import { onMounted } from 'vue';

const { orders, getOrders, isSubmitted,
  isAwaitingValidation,
  isStockConfirmed,
  isPaid,
  isShipped,
  isCancelled, } = useAdminOrders();

onMounted(async () => {
  await getOrders();
});

const getOrderStatusSeverity = (order: Order) => {
  if (isSubmitted(order)) {
    return 'info';
  } else if (isAwaitingValidation(order)) {
    return 'warn';
  } else if (isStockConfirmed(order)) {
    return 'success';
  } else if (isPaid(order)) {
    return 'success';
  } else if (isShipped(order)) {
    return 'success';
  } else if (isCancelled(order)) {
    return 'danger';
  }
  return '';
};

</script>

<template>
  <Fluid class="grid grid-cols-12 gap-8">
    <div class="col-span-12 xl:col-span-6">
      <h2 class="col-span-12 text-2xl font-bold mb-4">Orders</h2>
      <div class="card">
        <DataTable :value="orders" showGridlines tableStyle="min-width: 50rem">
          <Column field="orderNumber" header="Order Number"></Column>
          <Column field="date" header="Date"></Column>
          <Column field="total" header="Total"></Column>
          <Column header="Status">
            <template #body="slotProps">
              <Tag :value="slotProps.data.status" :severity="getOrderStatusSeverity(slotProps.data)" />
            </template>
          </Column>
          <Column field="city" header="City"></Column>
          <Column field="country" header="Country"></Column>
          <Column field="state" header="State"></Column>
          <Column field="street" header="Country"></Column>
          <Column field="zipcode" header="ZIP code"></Column>
        </DataTable>
      </div>
    </div>
  </Fluid>
</template>
