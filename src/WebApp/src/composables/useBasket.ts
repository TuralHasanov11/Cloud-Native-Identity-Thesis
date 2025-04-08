import type { BasketGrpcItem, BasketItem, Cart, CustomerBasketResponse } from '@/types/basket'
import { computed, ref, watch } from 'vue'

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

  async function getBasket(): Promise<BasketGrpcItem[]> {
    const { data } = await useBffFetch('/api/basket/basket').json<CustomerBasketResponse>()

    return data.value?.items ?? []
  }

  async function loadBasket(): Promise<void> {
    try {
      const loadedBasketItems: BasketGrpcItem[] = await getBasket()
      const productIds = loadedBasketItems.map((item) => item.productId)
      const catalog = useCatalog()

      const products = await catalog.getProductsByIds(productIds)
      const productMap = new Map(products.map((product) => [product.id, product]))

      const basketItems: BasketItem[] = []

      loadedBasketItems.forEach((item) => {
        const product = productMap.get(item.productId)
        if (product) {
          const updatedItem: BasketItem = {
            productId: product.id,
            productName: product.name,
            unitPrice: product.price,
            quantity: item.quantity,
          }

          basketItems.push(updatedItem)
        }

        cart.value.items = basketItems
      })
    } catch (error: unknown) {
      console.error('Error deleting basket:', error)
    } finally {
      isUpdatingCart.value = false
    }
  }

  async function deleteBasket(): Promise<boolean> {
    try {
      await useBffFetch('/api/basket/basket').delete()
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
      console.log(cart.value)
      const items = cart.value.items.map((item) => ({
        productId: item.productId,
        quantity: item.quantity,
      })) as BasketGrpcItem[]

      await useBffFetch('/api/basket/basket').post({
        items,
      })

      return true
    } catch (error: unknown) {
      console.error('Error updating basket:', error)
      return false
    } finally {
      isUpdatingCart.value = false
    }
  }

  //   function updatePaymentGateways(payload: PaymentGateways): void {
  //     paymentGateways.value = payload;
  //   }

  function toggleCart(state: boolean | undefined = undefined): void {
    isShowingCart.value = state ?? !isShowingCart.value
  }

  async function addToCart(item: BasketItem): Promise<void> {
    isUpdatingCart.value = true

    try {
      await loadBasket()

      const currentItem = cart.value.items.find((i) => i.productId === item.productId)
      if (currentItem) {
        currentItem.quantity += item.quantity
      } else {
        cart.value.items.push(item)
      }

      await updateBasket()

      if (!isShowingCart.value) toggleCart(true)
    } catch (error: unknown) {
      console.error('Error adding item to cart:', error)
      isUpdatingCart.value = false
    }
  }

  async function removeFromCart(productId: string) {
    isUpdatingCart.value = true
    cart.value.items = cart.value.items.filter((item) => item.productId !== productId)
    await updateBasket()
  }

  async function updateItemQuantity(productId: string, quantity: number): Promise<void> {
    isUpdatingCart.value = true

    try {
      await loadBasket()

      const currentItem = cart.value.items.find((i) => i.productId === productId)

      if (currentItem) {
        if (quantity > 0) {
          currentItem.quantity = quantity
        } else {
          cart.value.items = cart.value.items.filter((i) => i.productId !== productId)
        }

        await updateBasket()
      }

      // if (!isShowingCart.value) toggleCart(true)
    } catch (error: unknown) {
      console.error('Error adding item to cart:', error)
      isUpdatingCart.value = false
    }
  }

  async function emptyCart(): Promise<void> {
    try {
      isUpdatingCart.value = true

      cart.value.items = []
      await deleteBasket()
    } catch (error: unknown) {
      console.error('Error emptying cart:', error)
    }
  }

  watch(cart, () => {
    isUpdatingCart.value = false
  })

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
    loadBasket,
    isEmpty,
    productCount,
    total,
  }
}
