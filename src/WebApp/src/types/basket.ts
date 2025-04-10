export interface Cart {
  items: BasketItem[]
}

export type BasketItem = {
  id: string
  productId: string
  productName: string
  unitPrice: number
  oldUnitPrice?: number
  quantity: number
  pictureUrl: string
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
