<script lang="ts" setup>
import type { Product } from '@/types/catalog'
import { ref } from 'vue'

defineProps<{
  mainImage: string
  gallery: string[]
  product: Product
}>()

const responsiveOptions = ref([
  {
    breakpoint: '1400px',
    numVisible: 2,
    numScroll: 1,
  },
  {
    breakpoint: '1199px',
    numVisible: 3,
    numScroll: 1,
  },
  {
    breakpoint: '767px',
    numVisible: 2,
    numScroll: 1,
  },
  {
    breakpoint: '575px',
    numVisible: 1,
    numScroll: 1,
  },
])
</script>

<template>
  <Carousel :values="gallery" :numVisible="3" :numScroll="1" :responsiveOptions="responsiveOptions">
    <template #item="{ data }">
      <img :src="data" width="234" height="234" class="rounded-lg" />
    </template>
  </Carousel>
</template>
