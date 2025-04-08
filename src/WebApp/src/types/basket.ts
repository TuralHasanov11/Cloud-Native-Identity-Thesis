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
  items: BasketGrpcItem[]
}

export type BasketGrpcItem = {
  productId: string
  quantity: number
}

export type CustomerBasketResponse = {
  items: BasketGrpcItem[]
}

export type BasketCheckoutInfo = {
  street: string
  city: string
  state: string
  country: string
  cardTypeId: number
  zipcode: string
}
