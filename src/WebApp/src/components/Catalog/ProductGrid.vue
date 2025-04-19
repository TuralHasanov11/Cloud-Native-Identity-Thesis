<script setup lang="ts">
import type { Product } from '@/types/catalog';


defineProps<{ products: Product[] }>();

</script>

<template>
  <Transition name="fade" mode="out-in">
    <section v-if="products.length > 0" class="relative w-full">
      <TransitionGroup name="shrink" tag="div" mode="in-out"
        class="product-grid my-4 min-h-[600px] grid transition-all gap-8 lg:my-8">
        <ProductCard v-for="(product, i) in products" :key="product.id" :product :index="i" />
      </TransitionGroup>
      <!-- <Pagination /> -->
    </section>
    <NoProductsFound v-else />
  </Transition>
</template>

<style lang="css" scoped>
.product-grid {
  grid-template-columns: repeat(2, 1fr);
}

.product-grid:empty {
  display: none;
}

@media (min-width: 768px) {
  .product-grid {
    grid-template-columns: repeat(auto-fill, minmax(210px, 1fr));
  }
}

.shrink-move {
  transition: all 400ms;
}

.shrink-leave-active {
  transition: transform 300ms;
  position: absolute;
  opacity: 0;
}

.shrink-enter-active {
  transition:
    opacity 400ms ease-out 200ms,
    transform 400ms ease-out;
  will-change: opacity, transform;
}

.shrink-enter,
.shrink-leave-to,
.shrink-enter-from {
  opacity: 0;
  transform: scale(0.75) translateY(25%);
}
</style>
