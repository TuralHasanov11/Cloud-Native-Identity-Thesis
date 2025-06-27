<script setup lang="ts">
import ProductForm from '@/components/Admin/Catalog/ProductForm.vue';
import useAdminBrands from '@/composables/admin/catalog/useAdminBrands';
import useAdminProducts from '@/composables/admin/catalog/useAdminProducts';
import useAdminProductTypes from '@/composables/admin/catalog/useAdminProductTypes';
import { type ProductFormData } from '@/types/catalog';
import { mapToCreateProductRequest } from '@/utils/mapper';
import { useToast } from 'primevue/usetoast';
import { onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const router = useRouter();
const route = useRoute();
const toast = useToast()

const { updateProduct, product, getProduct } = useAdminProducts();
const { productTypes, getProductTypes } = useAdminProductTypes();
const { brands, getBrands } = useAdminBrands();

onMounted(async () => {
  const result = await getProduct(route.params.id as string);
  if (result.err) {
    toast.add({
      severity: 'error',
      summary: 'Product Not Found',
      detail: 'The product you are trying to update does not exist.',
      life: 3000,
    });
    router.push({ name: 'admin-catalog-products' });
  }
  await Promise.all([getBrands(), getProductTypes()])
});

async function onFormSubmit(data: ProductFormData) {
  const result = await updateProduct(product.value.id, mapToCreateProductRequest(data));
  if (result.ok) {
    toast.add({
      severity: 'success',
      summary: 'Product Updated',
      detail: 'The product has been updated successfully.',
      life: 3000,
    })
  }
  else {
    toast.add({
      severity: 'error',
      summary: 'Product Update Error',
      detail: 'There was an error updating the product. Please try again.',
      life: 3000,
    })
  }
}
</script>

<template>
  <Fluid class="grid grid-cols-12 gap-8">
    <div class="col-span-12 xl:col-span-6">
      <Toast />
      <template v-if="product && brands && productTypes">
        <ProductForm ref="product-form" @submit="onFormSubmit" :brands :productTypes :product :isUpdateMode="true" />
      </template>
    </div>
  </Fluid>
</template>
