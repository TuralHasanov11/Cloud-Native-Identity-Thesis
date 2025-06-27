<script setup lang="ts">
import useAdminProducts from '@/composables/admin/catalog/useAdminProducts';
import { onMounted } from 'vue';
import { useToast } from 'primevue/usetoast';

const toast = useToast()
const { products, getProducts, deleteProduct } = useAdminProducts();

onMounted(async () => {
  await getProducts();
});

async function onDeleteProduct(id: string) {
  const result = await deleteProduct(id);
  if (result.ok) {
    await getProducts();
    toast.add({
      severity: 'success',
      summary: 'Product Deleted',
      detail: 'The product has been deleted successfully.',
      life: 3000,
    });
  } else {
    toast.add({
      severity: 'error',
      summary: 'Product Deletion Error',
      detail: 'There was an error deleting the product. Please try again.',
      life: 3000,
    });
  }
}

</script>

<template>
  <Fluid class="grid grid-cols-12 gap-8">
    <div class="col-span-12 xl:col-span-6">
      <div class="card">
        <h2 class="font-semibold text-xl mb-4">Products</h2>
        <DataTable :value="products.data" showGridlines tableStyle="min-width: 50rem">
          <Column field="name" header="Name"></Column>
          <Column field="price" header="Price"></Column>
          <Column field="availableStock" header="AvailableStock"></Column>
          <Column header="Actions">
            <template #body="slotProps">
              <RouterLink :to="`/admin/catalog/products/${slotProps.data.id}`">
                <Button label="Edit" icon="pi pi-pencil" class="p-button-text" />
              </RouterLink>
              <Button @click="onDeleteProduct(slotProps.data.id)" severity="danger" label="Delete" icon="pi pi-trash" class="p-button-text" />
            </template>
          </Column>
        </DataTable>
      </div>
    </div>

  </Fluid>
</template>
