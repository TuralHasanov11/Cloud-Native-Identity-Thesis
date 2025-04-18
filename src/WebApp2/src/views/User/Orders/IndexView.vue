<script lang="ts" setup>
import DefaultLayout from '@/layouts/DefaultLayout.vue'

const { getOrders, orders } = useCustomer()

await getOrders()


</script>

<template>
  <DefaultLayout>
    <main id="user-orders">
      <UContainer>
        <UCard>
          <template #header>
            <h3>Orders</h3>
          </template>

          <UTable :data="orders" class="flex-1" />

        </UCard>
      </UContainer>
    </main>
  </DefaultLayout>
</template>