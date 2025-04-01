export type Cart = {
    items: BasketItem[]
}

export type BasketItem = {
    productId: string
    quantity: number
}

export type UpdateBasketRequest = {
    items: BasketItem[]
}