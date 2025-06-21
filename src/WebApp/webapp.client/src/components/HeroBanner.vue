<script setup lang="ts"></script>
<template>
  <div class="relative mx-auto">
    <img
      width="1400"
      height="800"
      class="object-cover w-full h-[420px] lg:h-[560px] xl:h-[640px]"
      src="/images/hero.jpg"
      alt="Hero image"
      loading="eager"
      sizes="sm:100vw md:1400px"
      fetchpriority="high"
      preload
      placeholder-class="blur-xl"
    />
    <div
      class="container mx-auto absolute inset-0 flex flex-col items-start justify-center bg-gradient-to-l from-gray-200 md:bg-none"
    >
      <h1 class="text-3xl font-bold md:mb-4 md:text-4xl lg:text-6xl">Just landed.</h1>
      <h2 class="text-lg font-bold md:mb-4 lg:text-3xl">The New Year Collection</h2>
      <div class="max-w-sm mb-8 text-md font-light lg:max-w-md text-balance">
        <p>
          Our latest collection is here. Discover the latest trends and styles for the new year.
        </p>
      </div>
      <RouterLink :to="{ name: 'products' }">Show Now</RouterLink>
    </div>
  </div>
</template>
