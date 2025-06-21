import type { BasketGrpcItem, BasketItem, Cart } from '@/types/basket'
import { HttpStatusCode } from '@/types/common'
import { computed, ref } from 'vue'
import useBffFetch from './useBffFetch'

export const DEFAULT_CART: Cart = {
  items: [],
}

const cart = ref<Cart>(DEFAULT_CART)

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

const isShowingCart = ref<boolean>(false)
const isUpdatingCart = ref<boolean>(false)
const isUpdatingCoupon = ref<boolean>(false)

export default function useBasket() {
  //   const paymentGateways = useState<PaymentGateways | null>('paymentGateways', () => null);

  async function getBasket(): Promise<void> {
    try {
      const { data } = await useBffFetch('/api/basket').json<BasketItem[]>()

      cart.value.items = data.value ?? []
    } catch (error: unknown) {
      console.error('Error fetching basket:', error)
      cart.value = DEFAULT_CART
    }
  }

  async function deleteBasket(): Promise<boolean> {
    isUpdatingCart.value = true
    try {
      await useBffFetch('/api/basket').delete()
      cart.value = DEFAULT_CART
      return true
    } catch (error: unknown) {
      console.error('Error deleting basket:', error)
      return false
    } finally {
      isUpdatingCart.value = false
    }
  }

  async function updateBasket(): Promise<boolean> {
    try {
      const items: BasketGrpcItem[] = cart.value.items.map((item) => ({
        productId: item.productId,
        quantity: item.quantity,
      }))

      console.log(items)

      const response = await useBffFetch('/api/basket')
        .post({
          items,
        })
        .json<BasketItem[]>()

      if (response.statusCode.value === HttpStatusCode.OK && response.data.value) {
        cart.value.items = response.data.value
        return true
      }
      return false
    } catch (error: unknown) {
      console.error('Error updating basket:', error)
      return false
    }
  }

  //   function updatePaymentGateways(payload: PaymentGateways): void {
  //     paymentGateways.value = payload;
  //   }

  function toggleCart(state: boolean | undefined = false): void {
    isShowingCart.value = state ?? !isShowingCart.value
  }

  async function addToCart(item: BasketItem): Promise<void> {
    isUpdatingCart.value = true

    try {
      const currentItem = cart.value.items.find((i) => i.productId === item.productId)
      if (currentItem) {
        currentItem.quantity += item.quantity
      } else {
        cart.value.items.push(item)
      }

      if (await updateBasket()) {
        if (!isShowingCart.value) {
          toggleCart(true)
        }
      }
    } catch (error: unknown) {
      console.error('Error adding item to cart:', error)
    } finally {
      isUpdatingCart.value = false
    }
  }

  async function removeFromCart(productId: string) {
    isUpdatingCart.value = true
    try {
      cart.value.items = cart.value.items.filter((item) => item.productId !== productId)
      await updateBasket()
    } catch (error: unknown) {
      console.error('Error removing from cart:', error)
    } finally {
      isUpdatingCart.value = false
    }
  }

  async function updateItemQuantity(productId: string, quantity: number): Promise<void> {
    isUpdatingCart.value = true

    try {
      const currentItem = cart.value.items.find((i) => i.productId === productId)

      if (currentItem) {
        if (quantity > 0) {
          currentItem.quantity = quantity
        } else {
          cart.value.items = cart.value.items.filter((i) => i.productId !== productId)
        }

        await updateBasket()
      }
    } catch (error: unknown) {
      console.error('Error adding item to cart:', error)
    } finally {
      isUpdatingCart.value = false
    }
  }

  async function emptyCart(): Promise<void> {
    isUpdatingCart.value = true

    try {
      cart.value = DEFAULT_CART
      await deleteBasket()
    } catch (error: unknown) {
      console.error('Error emptying cart:', error)
    } finally {
      isUpdatingCart.value = false
    }
  }

  return {
    cart,
    isShowingCart,
    isUpdatingCart,
    isUpdatingCoupon,
    toggleCart,
    addToCart,
    removeItem: removeFromCart,
    updateItemQuantity,
    emptyCart,
    deleteBasket,
    updateBasket,
    totalQuantity,
    getBasket,
    isEmpty,
    productCount,
    total,
  }
}
