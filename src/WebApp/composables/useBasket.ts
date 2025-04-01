import type { BasketItem, Cart } from "~/types/basket";

export const CART_NULL_OBJECT: Cart = {
  items: [],
};

export default function useBasket() {
  const cart = useState<Cart>("cart", () => CART_NULL_OBJECT);
  const totalQuantity = computed(() => {
    return cart.value.items.reduce((total, item) => total + item.quantity, 0);
  }
  );

  const isShowingCart = useState<boolean>("isShowingCart", () => false);
  const isUpdatingCart = useState<boolean>("isUpdatingCart", () => false);
  const isUpdatingCoupon = useState<boolean>("isUpdatingCoupon", () => false);
  //   const paymentGateways = useState<PaymentGateways | null>('paymentGateways', () => null);

  async function getBasket(): Promise<void>{
    try {
      // TODO: Fetch the basket from the server
    } catch (error: unknown) {
      console.error("Error deleting basket:", error);
    } finally {
      isUpdatingCart.value = false;
    }
  }


  async function deleteBasket(): Promise<boolean> {
    try {
        // TODO: Delete the basket on the server
      cart.value = CART_NULL_OBJECT;
      return true;
    } catch (error: unknown) {
      console.error("Error deleting basket:", error);
      return false;
    } finally {
      isUpdatingCart.value = false;
    }
  }

  async function updateBasket(): Promise<boolean> {
    try {
      // TODO: Update the basket on the server
      return true;
    } catch (error: unknown) {
      console.error("Error updating basket:", error);
      return false;
    } finally {
      isUpdatingCart.value = false;
    }
  }

  //   function updatePaymentGateways(payload: PaymentGateways): void {
  //     paymentGateways.value = payload;
  //   }

  function toggleCart(state: boolean | undefined = undefined): void {
    isShowingCart.value = state ?? !isShowingCart.value;
  }

  async function addToCart(item: BasketItem): Promise<void> {
    isUpdatingCart.value = true;

    try {
      cart.value.items.push(item);
      await updateBasket();

      const { storeSettings } = useAppConfig();
      if (storeSettings.autoOpenCart && !isShowingCart.value) toggleCart(true);
    } catch (error: unknown) {
      console.error("Error adding item to cart:", error);
    }
  }

  async function removeFromCart(productId: string) {
    isUpdatingCart.value = true;
    cart.value.items = cart.value.items.filter(
      (item) => item.productId !== productId
    );
    await updateBasket();
  }

  async function updateItemQuantity(
    productId: string,
    quantity: number
  ): Promise<void> {
    isUpdatingCart.value = true;
    try {
        const itemIndex = cart.value.items.findIndex(
            (item) => item.productId === productId
        );

        if (itemIndex !== -1) {
            cart.value.items[itemIndex].quantity = quantity;
        } else {
            console.error("Item not found in cart:", productId);
        }

      await updateBasket();
    } catch (error: unknown) {
        console.error("Error updating item quantity:", error);
    }
  }

  async function emptyCart(): Promise<void> {
    try {
      isUpdatingCart.value = true;
      
      cart.value.items = [];
      await deleteBasket();

    } catch (error: unknown) {
        console.error("Error emptying cart:", error);
    }
  }

  watch(cart, (_: Cart) => {
    isUpdatingCart.value = false;
  });

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
    getBasket
  };
}
