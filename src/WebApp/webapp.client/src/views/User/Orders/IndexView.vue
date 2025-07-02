<script lang="ts" setup>
import useCustomer from '@/composables/ordering/useCustomer'
import type { Order } from '@/types/ordering'
import { formatCurrency } from '@/utils/currency-formatter'
import { formatDate } from '@/utils/date-formatter'

const { getOrders } = useCustomer()

const { data: orders } = await getOrders()
</script>

<template>
  <main id="user-orders">
    <div class="container mx-auto">
      <Card>
        <template #title> Orders </template>

        <template #content>
          <DataTable :value="orders" class="flex-1">
            <Column field="orderNumber" header="Order Number"></Column>
            <Column field="date" header="Date">
              <template #body="{ data }: { data: Order }">
                {{ formatDate(data.date) }}
              </template>
            </Column>
            <Column field="status" header="Status"></Column>
            <Column field="total" header="Total">
              <template #body="{ data }: { data: Order }">
                {{ formatCurrency(data.total) }}
              </template>
            </Column>
          </DataTable>
        </template>
      </Card>
    </div>
  </main>
</template>
