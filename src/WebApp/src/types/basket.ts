export interface Cart {
  items: BasketItem[]
}

export type BasketItem = {
  productId: string
  quantity: number
  productName: string
  unitPrice: number
  oldUnitPrice?: number
}

export type UpdateBasketRequest = {
  items: BasketItem[]
}
