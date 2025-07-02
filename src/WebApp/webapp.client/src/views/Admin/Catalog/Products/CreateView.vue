<script setup lang="ts">
import ProductForm from '@/components/Admin/Catalog/ProductForm.vue'
import { useRouter } from 'vue-router'

const router = useRouter()

function onProductSaved() {
  router.push({ name: 'admin-catalog-products' })
}
</script>

<template>
  <Fluid class="grid grid-cols-12 gap-8">
    <div class="col-span-12 xl:col-span-6">
      <Toast />
      <ProductForm @productSaved="onProductSaved" />
    </div>
  </Fluid>
</template>
