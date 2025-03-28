export type Product = {
    id: string
    slug: string
    name: string,
    description: string,
    price: number,
    productTypeId: string,
    brandId: string,
    availableStock: number,
    restockThreshold: number,
    maxStockThreshold: number
}

export type Category = {
    id: string
    name: string
    slug: string
}