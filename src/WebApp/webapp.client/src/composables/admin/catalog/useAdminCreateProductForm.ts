import type { Brand, ProductType } from '@/types/catalog'
import { zodResolver } from '@primevue/forms/resolvers/zod'
import { ref } from 'vue'
import { z } from 'zod'
import useAdminBrands from './useAdminBrands'
import useAdminProductTypes from './useAdminProductTypes'
import type { AutoCompleteCompleteEvent } from 'primevue/autocomplete'
import { useToast } from 'primevue/usetoast'
import type { FormSubmitEvent } from '@primevue/forms'
import useBffFetch from '@/composables/useBffFetch'
import { HttpStatusCode } from '@/types/common'

export default function useAdminCreateProductForm() {
  const { brands } = useAdminBrands()
  const selectedBrand = ref<Brand | null>(null)
  const autoFilteredBrands = ref<Brand[]>([])
  const { productTypes } = useAdminProductTypes()
  const selectedProductType = ref<ProductType | null>(null)
  const autoFilteredProductTypes = ref<ProductType[]>([])

  const initialValues = ref<{
    name: string
    description: string | null
    price: number
    brand: Brand | null
    productType: ProductType | null
    availableStock: number
    restockThreshold: number
    maxStockThreshold: number
  }>({
    name: '',
    description: null,
    price: 0,
    brand: null,
    productType: null,
    availableStock: 0,
    restockThreshold: 0,
    maxStockThreshold: 0,
  })

  const resolver = ref(
    zodResolver(
      z.object({
        name: z.string().min(1, { message: 'Name is required' }),
        description: z.string().nullable(),
        price: z.number().min(0, { message: 'Price must be a positive number' }),
        brand: z
          .object({
            id: z.string().min(1, { message: 'Brand ID is required' }),
            name: z.string().min(1, { message: 'Brand Name is required' }),
          })
          .required()
          .refine((val) => val !== null, { message: 'Brand is required' }),
        productType: z
          .object({
            id: z.string().min(1, { message: 'Product Type ID is required' }),
            name: z.string().min(1, { message: 'Product Type Name is required' }),
          })
          .required()
          .refine((val) => val !== null, { message: 'Product Type is required' }),
        availableStock: z.number().min(0, { message: 'Available Stock must be a positive number' }),
        restockThreshold: z.number().min(0, { message: 'Restock Threshold must be a positive number' }),
        maxStockThreshold: z.number().min(0, { message: 'Max Stock Threshold must be a positive number' }),
      }),
    ),
  )

  function searchBrand(event: AutoCompleteCompleteEvent) {
    setTimeout(() => {
      if (!event.query.trim().length) {
        autoFilteredBrands.value = [...brands.value]
      } else {
        autoFilteredBrands.value = brands.value.filter((brand) => {
          return brand.name.toLowerCase().startsWith(event.query.toLowerCase())
        })
      }
    }, 250)
  }

  function searchProductType(event: AutoCompleteCompleteEvent) {
    setTimeout(() => {
      if (!event.query.trim().length) {
        autoFilteredProductTypes.value = [...productTypes.value]
      } else {
        autoFilteredProductTypes.value = productTypes.value.filter((productType) => {
          return productType.name.toLowerCase().startsWith(event.query.toLowerCase())
        })
      }
    }, 250)
  }

  function resetForm() {
    initialValues.value = {
      name: '',
      description: null,
      price: 0,
      brand: null,
      productType: null,
      availableStock: 0,
      restockThreshold: 0,
      maxStockThreshold: 0,
    }
    selectedBrand.value = null
    selectedProductType.value = null
    autoFilteredBrands.value = []
    autoFilteredProductTypes.value = []
  }

  async function onFormSubmit(event: FormSubmitEvent): Promise<boolean> {
    const toast = useToast()

    if (!event.valid) {
      toast.add({
        severity: 'error',
        summary: 'Form Submission Error',
        detail: 'Please correct the errors in the form.',
        life: 3000,
      })
      console.log('Form is invalid:', event.errors)
      return false
    }

    console.log('Form submitted successfully:', event.values)
    const response = await useBffFetch('/api/catalog/products').post(event.values)

    if (response.statusCode.value === HttpStatusCode.Created) {
      toast.add({
        severity: 'success',
        summary: 'Product Created',
        detail: 'The product has been created successfully.',
        life: 3000,
      })
      // Reset form values
      resetForm()
      return true
    } else {
      toast.add({
        severity: 'error',
        summary: 'Product Creation Error',
        detail: 'There was an error creating the product. Please try again.',
        life: 3000,
      })
      console.error('Error creating product:', response.error.value)
      return false
    }
  }

  return {
    initialValues,
    resolver,
    selectedBrand,
    autoFilteredBrands,
    searchBrand,
    selectedProductType,
    autoFilteredProductTypes,
    searchProductType,
    brands,
    onFormSubmit,
  }
}
