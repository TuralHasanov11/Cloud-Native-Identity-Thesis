<script setup lang="ts">
import { useHelpers } from '@/composables/useHelpers'
import type { Brand } from '@/types/catalog'
import { useImage } from '@vueuse/core'

const { FALLBACK_IMG } = useHelpers()

const { category } = defineProps<{
  category: Brand
}>()
const { isLoading: isImageLoading } = useImage({ src: FALLBACK_IMG })
const imgWidth = 300
const imgHeight = Math.round(imgWidth * 1.125)
</script>

<template>
  <RouterLink v-if="category" :to="`/product-category/${decodeURIComponent(category.id)}`"
    class="relative flex justify-center overflow-hidden border border-white rounded-xl item snap-mandatory snap-x">
    <img v-if="!isImageLoading" :src="FALLBACK_IMG" :width="imgWidth" :height="imgHeight"
      class="absolute inset-0 object-cover w-full h-full" :alt="category.name" :title="category.name"
      :sizes="`sm:${imgWidth / 2}px md:${imgWidth}px`" placeholder-class="blur-xl" />
    <div class="absolute inset-x-0 bottom-0 opacity-50 bg-gradient-to-t from-black to-transparent h-1/2"></div>
    <span class="relative z-10 mt-auto mb-2 text-sm font-semibold text-white capitalize md:text-base md:mb-4">{{
      category.name }}</span>
  </RouterLink>
</template>

<style scoped>
.item {
  scroll-snap-align: start;
  scroll-snap-stop: always;
  aspect-ratio: 4 / 5;
}
</style>
