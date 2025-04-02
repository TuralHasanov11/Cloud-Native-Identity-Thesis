<script lang="ts" setup>
const { cart, toggleCart, isUpdatingCart, getBasket } = useBasket();

await getBasket();
</script>

<template>
  <main id="cart">
    <UContainer>
      <UPageBody>
        <EmptyCart
          v-if="cart && !cart.isEmpty"
          class="rounded-lg shadow-lg p-1.5 hover:bg-red-400 hover:text-white"
        />

        <div class="mt-8 text-center">
          {{ $t("messages.shop.cart") }}
          <span v-if="cart?.productCount"> ({{ cart?.productCount }}) </span>
        </div>

        <div v-if="cart && !cart.isEmpty">
          <ul class="flex flex-col flex-1 gap-4 p-6 overflow-y-scroll md:p-8">
            <CartCard v-for="item in cart.items" :key="item.productId" :item />
          </ul>
          <div class="px-8 mb-8">
            <NuxtLink
              class="block p-3 text-lg text-center text-white bg-gray-800 rounded-lg shadow-md justify-evenly hover:bg-gray-900"
              to="/checkout"
              @click.prevent="toggleCart()"
            >
              <span class="mx-2">{{ $t("messages.shop.checkout") }}</span>
              <span>{{ cart.total }}</span>
            </NuxtLink>
          </div>
        </div>

        <EmptyCartMessage v-else-if="cart && cart.isEmpty" />

        <div v-if="isUpdatingCart" class="absolute inset-0 flex items-center justify-center bg-white bg-opacity-25">
            <!-- <LoadingIcon /> -->
        </div>
      </UPageBody>
    </UContainer>
  </main>
</template>
