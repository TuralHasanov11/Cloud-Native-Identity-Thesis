<script setup lang="ts">
import useCatalog from '@/composables/useCatalog';

const { products } = useCatalog();
</script>

<template>
  <div v-if="products.data.length !== 0" class="text-sm font-light">
    <span>{{ $t("messages.shop.productResultCount.showing") + " " }}</span>
    (<span>{{ $t("messages.shop.productResultCount.of") }}</span>
    <span class="font-normal">{{ products.count }}</span>)
  </div>
</template>
