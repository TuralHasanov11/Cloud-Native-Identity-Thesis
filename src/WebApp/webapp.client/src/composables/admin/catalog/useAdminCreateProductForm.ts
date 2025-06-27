import type { Brand, Product, ProductFormData, ProductType } from '@/types/catalog'
import { zodResolver } from '@primevue/forms/resolvers/zod'
import type { AutoCompleteCompleteEvent } from 'primevue/autocomplete'
import { ref, watch } from 'vue'
import { z } from 'zod'

const DEFAULT_INITIAL_VALUES: ProductFormData = {
  name: '',
  description: null,
  price: 0,
  brand: null,
  productType: null,
  availableStock: 0,
  restockThreshold: 0,
  maxStockThreshold: 0,
}

export default function useAdminCreateProductForm({
  product,
  brands,
  productTypes,
}: {
  product?: Product
  brands: Brand[]
  productTypes: ProductType[]
}) {
  const selectedBrand = ref<Brand | null>(null)
  const autoFilteredBrands = ref<Brand[]>([])
  const selectedProductType = ref<ProductType | null>(null)
  const autoFilteredProductTypes = ref<ProductType[]>([])

  const initialValues = ref<ProductFormData>(
    product
      ? {
          name: product.name,
          description: product.description,
          price: product.price,
          brand: brands.find((b) => b.id === product.brandId) || null,
          productType: productTypes.find((pt) => pt.id === product.productTypeId) || null,
          availableStock: product.availableStock,
          restockThreshold: product.restockThreshold,
          maxStockThreshold: product.maxStockThreshold,
        }
      : { ...DEFAULT_INITIAL_VALUES },
  )

  const resolver = ref(
    zodResolver(
      z.object({
        name: z.string().min(1, { message: 'Name is required' }),
        description: z.string().nullable(),
        price: z.number().min(0, { message: 'Price must be a positive number' }),
        brand: z.union([
          z.object({
            id: z.string().min(1, { message: 'Brand ID is required' }),
            name: z.string().min(1, { message: 'Brand Name is required' }),
          }),
          z.any().refine((val) => val !== null, { message: 'Brand is required.' }),
        ]),
        productType: z.union([
          z.object({
            id: z.string().min(1, { message: 'Product Type ID is required' }),
            name: z.string().min(1, { message: 'Product Type Name is required' }),
          }),
          z.any().refine((val) => val !== null, { message: 'Product Type is required' }),
        ]),
        availableStock: z.number().min(0, { message: 'Available Stock must be a positive number' }),
        restockThreshold: z.number().min(0, { message: 'Restock Threshold must be a positive number' }),
        maxStockThreshold: z.number().min(0, { message: 'Max Stock Threshold must be a positive number' }),
      }),
    ),
  )

  function searchBrand(event: AutoCompleteCompleteEvent) {
    setTimeout(() => {
      if (!event.query.trim().length) {
        autoFilteredBrands.value = [...brands]
      } else {
        autoFilteredBrands.value = brands.filter((brand) => {
          return brand.name.toLowerCase().startsWith(event.query.toLowerCase())
        })
      }
    }, 250)
  }

  function searchProductType(event: AutoCompleteCompleteEvent) {
    setTimeout(() => {
      if (!event.query.trim().length) {
        autoFilteredProductTypes.value = [...productTypes]
      } else {
        autoFilteredProductTypes.value = productTypes.filter((productType) => {
          return productType.name.toLowerCase().startsWith(event.query.toLowerCase())
        })
      }
    }, 250)
  }

  function reset() {
    initialValues.value = DEFAULT_INITIAL_VALUES
    selectedBrand.value = null
    selectedProductType.value = null
    autoFilteredBrands.value = []
    autoFilteredProductTypes.value = []
  }

  watch(brands, (newBrands) => {
    if (newBrands.length > 0) {
      autoFilteredBrands.value = [...newBrands]
    }
  })

  watch(productTypes, (newProductTypes) => {
    if (newProductTypes.length > 0) {
      autoFilteredProductTypes.value = [...newProductTypes]
    }
  })

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
    reset,
  }
}
