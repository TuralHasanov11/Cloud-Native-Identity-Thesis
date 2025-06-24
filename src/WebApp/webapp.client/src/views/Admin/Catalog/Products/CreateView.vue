<script setup lang="ts">
import useAdminCreateProductForm from '@/composables/admin/catalog/useAdminCreateProductForm';



const {
  resolver,
  initialValues,
  onFormSubmit,
  autoFilteredBrands,
  autoFilteredProductTypes,
  searchBrand,
  searchProductType,
  selectedBrand,
  selectedProductType
} = useAdminCreateProductForm();



</script>

<template>
  <Fluid class="grid grid-cols-12 gap-8">
    <h1 class="col-span-12 text-2xl font-bold mb-4">Create Product</h1>
    <div class="col-span-12 xl:col-span-6">
      <Form v-slot="$form" :initialValues="initialValues" :resolver="resolver" @submit="onFormSubmit"
        class="card flex flex-col gap-4">
        <div class="font-semibold text-xl">Vertical</div>
        <div class="flex flex-col gap-2">
          <label for="name">Name</label>
          <InputText id="name" type="text" />
          <Message v-if="$form.name?.invalid" severity="error" size="small" variant="simple">{{ $form.name.error.message
            }}</Message>
        </div>
        <div class="flex flex-col gap-2">
          <label for="description">Description</label>
          <Textarea id="description" type="text" />
          <Message v-if="$form.description?.invalid" severity="error" size="small" variant="simple">{{
            $form.description.error.message
          }}</Message>
        </div>
        <div class="flex flex-col gap-2">
          <label for="price">Price</label>
          <InputNumber id="price" type="number" />
          <Message v-if="$form.price?.invalid" severity="error" size="small" variant="simple">{{
            $form.price.error.message
          }}</Message>
        </div>
        <div class="flex flex-col gap-2">
          <label for="brand">Brand</label>
          <AutoComplete id="brand" v-model="selectedBrand" :suggestions="autoFilteredBrands" optionLabel="brand"
            placeholder="Search" dropdown multiple display="chip" @complete="searchBrand($event)" />
          <Message v-if="$form.brand?.invalid" severity="error" size="small" variant="simple">{{
            $form.brand.error.message
          }}</Message>
        </div>
        <div class="flex flex-col gap-2">
          <label for="productType">Product Type</label>
          <AutoComplete id="productType" v-model="selectedProductType" :suggestions="autoFilteredProductTypes"
            optionLabel="productType" placeholder="Search" dropdown multiple display="chip"
            @complete="searchProductType($event)" />
          <Message v-if="$form.productType?.invalid" severity="error" size="small" variant="simple">{{
            $form.productType.error.message
          }}</Message>
        </div>
        <div class="flex flex-col gap-2">
          <label for="availableStock">Available Stock</label>
          <InputNumber id="availableStock" type="number" />
          <Message v-if="$form.availableStock?.invalid" severity="error" size="small" variant="simple">{{
            $form.availableStock.error.message
          }}</Message>
        </div>
        <div class="flex flex-col gap-2">
          <label for="restockThreshold">Restock Threshold</label>
          <InputNumber id="restockThreshold" type="number" />
          <Message v-if="$form.restockThreshold?.invalid" severity="error" size="small" variant="simple">{{
            $form.restockThreshold.error.message
          }}</Message>
        </div>
        <div class="flex flex-col gap-2">
          <label for="maxStockThreshold">Max Stock Threshold</label>
          <InputNumber id="maxStockThreshold" type="number" />
          <Message v-if="$form.maxStockThreshold?.invalid" severity="error" size="small" variant="simple">{{
            $form.maxStockThreshold.error.message
          }}</Message>
        </div>

        <Button type="submit" label="Submit" :fluid="false"></Button>
      </Form>
    </div>

  </Fluid>
</template>
