import type { BasketItem, Cart } from '@/types/basket'
import { HttpStatusCode } from '@/types/common'
import { Err, Ok } from 'ts-results'
import { computed, readonly, ref } from 'vue'
import useIdentity from '../identity/useIdentity'
import useBffFetch from '../useBffFetch'

const DEFAULT_CART: Cart = Object.freeze({
  items: [],
})

const cart = ref<Cart>({ ...DEFAULT_CART })
const isInitialized = ref<boolean>(false)

const totalQuantity = computed<number>(() => {
  return cart.value.items.reduce((total, item) => total + item.quantity, 0)
})

const isEmpty = computed<boolean>(() => {
  return cart.value.items.length === 0
})

const productCount = computed<number>(() => {
  return cart.value.items.reduce((total, item) => total + item.quantity, 0)
})

const total = computed<number>(() => {
  return cart.value.items.reduce((total, item) => total + item.unitPrice * item.quantity, 0)
})

const isUpdatingCart = ref<boolean>(false)
const isShowingCart = ref<boolean>(false)

export default function useBasket() {
  const { isAuthenticated } = useIdentity()

  async function getBasket(): Promise<void> {
    const { data } = await useBffFetch('/api/basket').json<BasketItem[]>()
    if (data.value) {
      cart.value.items = data.value
    }
  }

  async function deleteBasket() {
    const response = await useBffFetch('/api/basket').delete()

    if (response.statusCode.value !== HttpStatusCode.NoContent) {
      return new Err('Failed to delete basket')
    }

    return Ok.EMPTY
  }

  async function updateBasket(items: BasketItem[]) {
    const { data, statusCode } = await useBffFetch('/api/basket')
      .post({
        items: items.map((item) => ({
          productId: item.productId,
          quantity: item.quantity,
        })),
      })
      .json<BasketItem[]>()

    if (statusCode.value === HttpStatusCode.OK && data.value) {
      await getBasket()
      return Ok.EMPTY
    }
    return new Err('Failed to update basket')
  }

  function toggleCart(state: boolean | undefined = false): void {
    isShowingCart.value = state ?? !isShowingCart.value
  }

  async function addToCart(item: BasketItem) {
    isUpdatingCart.value = true
    const updatedCartItems = [...cart.value.items]
    const currentItem = updatedCartItems.find((i) => i.productId === item.productId)
    if (currentItem) {
      currentItem.quantity += item.quantity
    } else {
      updatedCartItems.push(item)
    }

    const result = await updateBasket(updatedCartItems)
    if (result.ok) {
      if (!isShowingCart.value) {
        toggleCart(true)
      }
      isUpdatingCart.value = false
      return Ok.EMPTY
    }

    isUpdatingCart.value = false
    return new Err('Failed to add item to cart')
  }

  async function removeItem(productId: string) {
    const updatedCart = cart.value.items.filter((item) => item.productId !== productId)
    const result = await updateBasket(updatedCart)
    if (result.ok) {
      return Ok.EMPTY
    }

    return new Err('Failed to remove item from cart')
  }

  async function updateItemQuantity(productId: string, quantity: number) {
    isUpdatingCart.value = true

    try {
      let updatedCartItems = [...cart.value.items]
      const currentItem = updatedCartItems.find((i) => i.productId === productId)

      if (currentItem) {
        if (quantity > 0) {
          updatedCartItems = updatedCartItems.map((i) => (i.productId === productId ? { ...i, quantity } : i))
        } else {
          updatedCartItems = cart.value.items.filter((i) => i.productId !== productId)
        }

        await updateBasket(updatedCartItems)
        return Ok.EMPTY
      }
    } catch (error: unknown) {
      console.error('Error adding item to cart:', error)
      return new Err('Failed to update item quantity in cart')
    } finally {
      isUpdatingCart.value = false
    }
  }

  async function emptyCart() {
    isUpdatingCart.value = true

    const result = await deleteBasket()
    if (result.ok) {
      cart.value = { ...DEFAULT_CART }
      console.log(cart.value, DEFAULT_CART)
      isUpdatingCart.value = false
      return Ok.EMPTY
    }
    isUpdatingCart.value = false
    return new Err('Failed to empty cart')
  }

  async function initializeBasket() {
    if (isAuthenticated.value && !isInitialized.value) {
      await getBasket()
      isInitialized.value = true
    }
  }

  return {
    cart: readonly(cart),
    isShowingCart: readonly(isShowingCart),
    isUpdatingCart: readonly(isUpdatingCart),
    toggleCart,
    addToCart,
    removeItem,
    updateItemQuantity,
    emptyCart,
    totalQuantity,
    isEmpty,
    productCount,
    total,
    initializeBasket,
  }
}
