<script setup lang="ts">
import useBasket from '@/composables/basket/useBasket'

const { emptyCart } = useBasket()
</script>

<template>
  <Button icon="pi pi-times" color="error" class="cursor-pointer top-6 right-6 md:right-8 absolute"
    title="Empty Cart" @click="emptyCart">
  </Button>
</template>
