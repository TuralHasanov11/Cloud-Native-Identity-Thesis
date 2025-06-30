import type { PaginationResponse } from './pagination'

export type Product = {
  id: string
  name: string
  description: string
  price: number
  productTypeId: string
  brandId: string
  availableStock: number
  restockThreshold: number
  maxStockThreshold: number
  categories?: Category[]
  pictureUrl?: string
}

export type Category = {
  id: string
  slug: string
  name: string
}

export type CreateOrUpdateProductRequest = {
  name: string
  description: string
  price: number
  productTypeId: string
  brandId: string
  availableStock: number
  restockThreshold: number
  maxStockThreshold: number
}

export type UpdateProductRequest = {
  name: string
  description: string
  price: number
  productTypeId: string
  brandId: string
  availableStock: number
  restockThreshold: number
  maxStockThreshold: number
}

export type Brand = {
  id: string
  name: string
}

export type ProductType = {
  id: string
  name: string
}

export enum StockStatusEnum {
  OUT_OF_STOCK,
  IN_STOCK,
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

export const PRODUCT_NULL_OBJECT: Product = {
  id: '',
  name: '',
  description: '',
  price: 0,
  productTypeId: '',
  brandId: '',
  availableStock: 0,
  restockThreshold: 0,
  maxStockThreshold: 0,
  categories: [],
}

export interface ProductFormData {
  name: string
  description: string | null
  price: number
  brand: Brand | null
  productType: ProductType | null
  availableStock: number
  restockThreshold: number
  maxStockThreshold: number
}
