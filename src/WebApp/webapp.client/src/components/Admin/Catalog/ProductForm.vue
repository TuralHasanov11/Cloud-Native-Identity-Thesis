<script lang="ts" setup>
import useAdminBrands from '@/composables/admin/catalog/useAdminBrands'
import useAdminCreateProductForm from '@/composables/admin/catalog/useAdminCreateProductForm'
import useAdminProducts from '@/composables/admin/catalog/useAdminProducts'
import useAdminProductTypes from '@/composables/admin/catalog/useAdminProductTypes'
import { PRODUCT_NULL_OBJECT, type Product, type ProductFormData } from '@/types/catalog'
import { mapToCreateOrUpdateProductRequest } from '@/utils/mapper'
import type { FormSubmitEvent } from '@primevue/forms'
import { useToast } from 'primevue'
import { computed } from 'vue'

const { product = PRODUCT_NULL_OBJECT } = defineProps<{
  product?: Product
}>()

const emit = defineEmits<{
  productSaved: []
}>()

const { createProduct, updateProduct } = useAdminProducts()
const { getProductTypes } = useAdminProductTypes()
const { getBrands } = useAdminBrands()
const toast = useToast()

const { data: productTypes } = await getProductTypes()
const { data: brands } = await getBrands()

const isUpdateMode = computed(() => product?.id != null && product?.id !== PRODUCT_NULL_OBJECT.id)

const {
  resolver,
  initialValues,
  autoFilteredBrands,
  autoFilteredProductTypes,
  searchBrand,
  searchProductType,
  selectedBrand,
  selectedProductType,
  reset,
} = useAdminCreateProductForm({
  product,
  brands: brands?.value ?? [],
  productTypes: productTypes?.value ?? [],
})

async function onFormSubmit(event: FormSubmitEvent) {
  if (!event.valid) {
    return
  }

  const formData = mapToCreateOrUpdateProductRequest(event.values as ProductFormData)

  if (isUpdateMode.value && product) {
    const result = await updateProduct(product.id, formData)
    if (result.ok) {
      notifySuccess()
      emit('productSaved')
    } else {
      notifyFail()
    }
  } else {
    const result = await createProduct(formData)
    if (result.ok) {
      reset()
      notifySuccess()
      emit('productSaved')
    } else {
      notifyFail()
    }
  }
}

function notifySuccess() {
  toast.add({
    severity: 'success',
    summary: isUpdateMode.value ? 'Product Updated' : 'Product Created',
    detail: 'The product has been updated successfully.',
    life: 3000,
  })
}

function notifyFail() {
  toast.add({
    severity: 'error',
    summary: isUpdateMode.value ? 'Product Update Error' : 'Product Creation Error',
    detail: 'There was an error creating the product. Please try again.',
    life: 3000,
  })
}
</script>

<template>
  <Form v-slot="$form" :initialValues="initialValues" :resolver="resolver" @submit="onFormSubmit" class="card flex flex-col gap-4">
    <div class="font-semibold text-xl">
      {{ isUpdateMode ? 'Update Product' : 'Create Product' }}
    </div>
    <div class="flex flex-col gap-2">
      <label for="name">Name</label>
      <InputText id="name" name="name" type="text" />
      <Message v-if="$form.name?.invalid" severity="error" size="small" variant="simple">{{ $form.name.error.message }} </Message>
    </div>
    <div class="flex flex-col gap-2">
      <label for="description">Description</label>
      <Textarea id="description" name="description" type="text" />
      <Message v-if="$form.description?.invalid" severity="error" size="small" variant="simple">{{ $form.description.error.message }}</Message>
    </div>
    <div class="flex flex-col gap-2">
      <label for="price">Price</label>
      <InputNumber id="price" name="price" type="number" />
      <Message v-if="$form.price?.invalid" severity="error" size="small" variant="simple">{{ $form.price.error.message }}</Message>
    </div>
    <div class="flex flex-col gap-2">
      <label for="brand">Brand</label>
      <AutoComplete
        id="brand"
        name="brand"
        v-model="selectedBrand"
        :suggestions="autoFilteredBrands"
        optionLabel="name"
        placeholder="Search"
        dropdown
        @complete="searchBrand($event)"
      />
      <Message
        v-if="
          // @ts-expect-error: brand.id may not exist on some types primevue leads to compile errors
          $form.brand?.id?.invalid
        "
        severity="error"
        size="small"
        variant="simple"
        >{{
          // @ts-expect-error: brand.id may not exist on some types primevue leads to compile errors
          $form.brand.id.error?.message
        }}</Message
      >
      <Message
        v-if="
          // @ts-expect-error: brand.name may not exist on some types primevue leads to compile errors
          $form.brand?.name?.invalid
        "
        severity="error"
        size="small"
        variant="simple"
        >{{
          // @ts-expect-error: brand.name may not exist on some types primevue leads to compile errors
          $form.brand.name.error?.message
        }}</Message
      >
    </div>
    <div class="flex flex-col gap-2">
      <label for="productType">Product Type</label>
      <AutoComplete
        id="productType"
        name="productType"
        v-model="selectedProductType"
        :suggestions="autoFilteredProductTypes"
        optionLabel="name"
        placeholder="Search"
        dropdown
        @complete="searchProductType($event)"
      />
      <Message
        v-if="
          // @ts-expect-error: productType.id may not exist on some types primevue leads to compile errors
          $form.productType?.id?.invalid
        "
        severity="error"
        size="small"
        variant="simple"
        >{{
          // @ts-expect-error: productType.id may not exist on some types primevue leads to compile errors
          $form.productType.id.error?.message
        }}</Message
      >
      <Message
        v-if="
          // @ts-expect-error: productType.name may not exist on some types primevue leads to compile errors
          $form.productType?.name?.invalid
        "
        severity="error"
        size="small"
        variant="simple"
        >{{
          // @ts-expect-error: productType.name may not exist on some types primevue leads to compile errors
          $form.productType.name.error?.message
        }}</Message
      >
    </div>
    <div class="flex flex-col gap-2">
      <label for="availableStock">Available Stock</label>
      <InputNumber id="availableStock" name="availableStock" type="number" />
      <Message v-if="$form.availableStock?.invalid" severity="error" size="small" variant="simple">{{ $form.availableStock.error.message }}</Message>
    </div>
    <div class="flex flex-col gap-2">
      <label for="restockThreshold">Restock Threshold</label>
      <InputNumber id="restockThreshold" name="restockThreshold" type="number" />
      <Message v-if="$form.restockThreshold?.invalid" severity="error" size="small" variant="simple">{{
        $form.restockThreshold.error.message
      }}</Message>
    </div>
    <div class="flex flex-col gap-2">
      <label for="maxStockThreshold">Max Stock Threshold</label>
      <InputNumber id="maxStockThreshold" name="maxStockThreshold" type="number" />
      <Message v-if="$form.maxStockThreshold?.invalid" severity="error" size="small" variant="simple">{{
        $form.maxStockThreshold.error.message
      }}</Message>
    </div>

    <Button :disabled="!$form.valid" type="submit" label="Submit" :fluid="false"></Button>
  </Form>
</template>
