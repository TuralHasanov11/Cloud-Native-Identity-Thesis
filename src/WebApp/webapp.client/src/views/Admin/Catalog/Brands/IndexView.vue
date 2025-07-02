<script setup lang="ts">
import useAdminBrands from '@/composables/admin/catalog/useAdminBrands'

const { getBrands } = useAdminBrands()

const { data: brands } = await getBrands()
</script>

<template>
  <Fluid class="grid grid-cols-12 gap-8">
    <div class="col-span-12 xl:col-span-6">
      <div class="card">
        <h2 class="font-semibold text-xl mb-4">Brands</h2>
        <DataTable :value="brands" showGridlines tableStyle="min-width: 25rem">
          <Column field="name" header="Name"></Column>
        </DataTable>
      </div>
    </div>
  </Fluid>
</template>
