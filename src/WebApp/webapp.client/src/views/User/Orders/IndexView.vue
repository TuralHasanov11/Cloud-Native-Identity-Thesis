<script lang="ts" setup>
import useCustomer from '@/composables/useCustomer'

const { orders } = useCustomer()


</script>

<template>
  <main id="user-orders">
    <div class="container mx-auto">
      <Card>
        <template #title> Orders </template>

        <template #content>
          <DataTable :value="orders" class="flex-1">
            <Column field="orderNumber" header="Order Number"></Column>
            <Column field="date" header="Date"></Column>
            <Column field="status" header="Status"></Column>
            <Column field="total" header="Total"></Column>
          </DataTable>
        </template>
      </Card>
    </div>
  </main>
</template>
