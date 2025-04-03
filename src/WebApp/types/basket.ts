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

// Jannis Modification

export type UpdateBasketRequest = {
    items: BasketItem[]
}


// Aytaj Modification