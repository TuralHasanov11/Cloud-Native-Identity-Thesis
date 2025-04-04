<script lang="ts" setup>

// const { isShowingCart, toggleCart } = useCart();

</script>

<template>
    <div>
      <BaseHeader />

      <!-- <Transition name="slide-from-right">
        <LazyCart v-if="isShowingCart" />
      </Transition> -->

      <!-- <Transition name="slide-from-left">
        <MobileMenu v-if="isShowingMobileMenu" />
      </Transition> -->
  
      <UMain>
        <slot />
      </UMain>

      <!-- <Transition name="fade">
      <div v-if="isShowingCart || isShowingMobileMenu" class="bg-black opacity-25 inset-0 z-40 fixed" @click="closeCartAndMenu" />
    </Transition> -->
  
      <LazyBaseFooter hydrate-on-interaction />
    </div>
  </template>
  