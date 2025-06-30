<script setup lang="ts">
const imgWidth = 220
const imgHeight = Math.round(imgWidth * 1.125)
</script>

<template>
  <div class="relative flex justify-center overflow-hidden border border-white rounded-xl item snap-mandatory snap-x">
    <Skeleton class="absolute inset-0 object-cover w-full h-full" :width="`${imgWidth}`" :height="`${imgHeight}`"> </Skeleton>
    <div class="absolute inset-x-0 bottom-0 opacity-50 bg-gradient-to-t from-black to-transparent h-1/2"></div>
    <span class="relative z-10 mt-auto mb-2 text-sm font-semibold text-white capitalize md:text-base md:mb-4">
      <Skeleton width="4rem" height="2rem"></Skeleton>
    </span>
  </div>
</template>

<style scoped>
.item {
  scroll-snap-align: start;
  scroll-snap-stop: always;
  aspect-ratio: 4 / 5;
}
</style>
