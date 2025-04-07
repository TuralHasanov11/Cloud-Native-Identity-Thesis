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

export type BasketGrpcItem = {
  product_id: string
  quantity: number
}

export type BasketCheckoutInfo = {
  street?: string
  city?: string
  state?: string
  country?: string
  cardTypeId: number
  customer?: string
  zipcode?: string
}
