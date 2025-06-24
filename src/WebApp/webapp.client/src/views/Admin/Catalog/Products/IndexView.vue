<script setup lang="ts">
import useAdminProducts from '@/composables/admin/catalog/useAdminProducts';
import { onMounted } from 'vue';

const { products, getProducts } = useAdminProducts();

onMounted(async () => {
  await getProducts();
});

</script>

<template>
  <Fluid class="grid grid-cols-12 gap-8">
    <h1 class="col-span-12 text-2xl font-bold mb-4">Products</h1>
    <div class="col-span-12 xl:col-span-6">
      <div class="card">
        <DataTable :value="products.data" showGridlines tableStyle="min-width: 50rem">
          <Column field="id" header="Id"></Column>
          <Column field="name" header="Name"></Column>
          <Column field="price" header="Price"></Column>
          <Column field="availableStock" header="AvailableStock"></Column>
        </DataTable>
      </div>
    </div>

  </Fluid>
</template>
