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
  categories: Category[]
  pictureUrl?: string
}

export type Category = {
  id: string
  slug: string
  name: string
}

export type CreateProductRequest = {
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
