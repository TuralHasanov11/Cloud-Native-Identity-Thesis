<script setup lang="ts">
import useAdminBrands from '@/composables/admin/catalog/useAdminBrands';
import { onMounted } from 'vue';

const { brands, getBrands } = useAdminBrands();

onMounted(async () => {
  await getBrands();
});

</script>

<template>
  <Fluid class="grid grid-cols-12 gap-8">
    <h1 class="col-span-12 text-2xl font-bold mb-4">Brands</h1>
    <div class="col-span-12 xl:col-span-6">
      <div class="card">
        <DataTable :value="brands" showGridlines tableStyle="min-width: 50rem">
          <Column field="id" header="Id"></Column>
          <Column field="name" header="Name"></Column>
        </DataTable>
      </div>
    </div>

  </Fluid>
</template>
