<script setup lang="ts">
const { products } = useCatalog();
</script>

<template>
  <div v-if="products.length !== 0" class="text-sm font-light">
    <span>{{ $t("messages.shop.productResultCount.showing") + " " }}</span>
    (<span>{{ $t("messages.shop.productResultCount.of") }}</span>
    <span class="font-normal">{{ products.length }}</span
    >)
  </div>
</template>
