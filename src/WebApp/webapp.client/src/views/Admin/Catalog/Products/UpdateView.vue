<script setup lang="ts">
import ProductForm from '@/components/Admin/Catalog/ProductForm.vue'
import useAdminProducts from '@/composables/admin/catalog/useAdminProducts'
import { useToast } from 'primevue/usetoast'
import { useRoute, useRouter } from 'vue-router'

const router = useRouter()
const route = useRoute()
const toast = useToast()

const { getProduct } = useAdminProducts()

const { data: product, isFetching: isFetchingProduct } = await getProduct(route.params.id as string)
if (product.value == null) {
  toast.add({
    severity: 'error',
    summary: 'Product Not Found',
    detail: 'The product you are trying to update does not exist.',
    life: 3000,
  })
  router.push({ name: 'admin-catalog-products' })
}
</script>

<template>
  <Fluid class="grid grid-cols-12 gap-8">
    <div class="col-span-12 xl:col-span-6">
      <Toast />
      <template v-if="product">
        <ProductForm v-if="isFetchingProduct" :product />
      </template>
    </div>
  </Fluid>
</template>
