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
