import { defineStore } from 'pinia'
import { shallowRef, type ShallowRef } from 'vue'
import type { Brand, Product, ProductType } from '@/types/catalog'
import type { PaginationResponse } from '@/types/pagination'
import useBffFetch from '@/composables/useBffFetch'

export interface CatalogStore {
  popularProducts: ShallowRef<Product[]>
  brands: ShallowRef<Brand[]>
  products: ShallowRef<PaginationResponse<Product, string>>
  productTypes: ShallowRef<ProductType[]>
  getBrands: () => Promise<void>
  getProductTypes: () => Promise<void>
  getProducts: (payload?: GetProductsRequest) => Promise<void>
}

export interface GetProductsRequest {
  name?: string
  productType?: string
  brand?: string
  pageSize?: number
  pageCursor?: string
}

export const PAGINATED_PRODUCTS_NULL_OBJECT: PaginationResponse<Product, string> = {
  data: [],
  pageSize: 0,
  pageCursor: '',
  count: 0,
}

export const useCatalogStore = defineStore('catalog', (): CatalogStore => {
  const popularProducts = shallowRef<Product[]>([])
  const brands = shallowRef<Brand[]>([])
  const products = shallowRef<PaginationResponse<Product, string>>(PAGINATED_PRODUCTS_NULL_OBJECT)
  const productTypes = shallowRef<ProductType[]>([])

  async function getProducts(payload?: GetProductsRequest): Promise<void> {
    const params = new URLSearchParams()

    if (payload?.brand) {
      params.append('brand', payload.brand)
    }

    if (payload?.productType) {
      params.append('type', payload.productType)
    }

    if (payload?.name) {
      params.append('name', payload.name)
    }

    if (payload?.pageSize) {
      params.append('pageSize', payload.pageSize.toString())
    }

    if (payload?.pageCursor) {
      params.append('pageCursor', payload.pageCursor)
    }

    const { data } = await useBffFetch(`/api/catalog/products?${params.toString()}`).json<
      PaginationResponse<Product, string>
    >()
    if (data.value) {
      products.value = data.value
    }
  }

  async function getBrands(): Promise<void> {
    const { data } = await useBffFetch('/api/catalog/brands').json<Brand[]>()
    if (data.value) {
      brands.value = data.value
    }
  }

  async function getProductTypes(): Promise<void> {
    const { data } = await useBffFetch<ProductType[]>('/api/catalog/product-types')

    if (data.value) {
      productTypes.value = data.value
    }
  }

  return {
    popularProducts,
    brands,
    products,
    productTypes,
    getBrands,
    getProductTypes,
    getProducts,
  }
})
