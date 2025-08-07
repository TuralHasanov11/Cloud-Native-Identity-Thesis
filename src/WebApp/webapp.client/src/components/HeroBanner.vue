<script setup lang="ts"></script>
<template>
  <div class="relative mx-auto">
    <img width="1400" height="800" class="object-cover w-full h-[420px] lg:h-[560px] xl:h-[640px]"
      src="/images/hero1.PNG" alt="Hero" loading="eager" sizes="sm:100vw md:1400px" fetchpriority="high" preload
      placeholder-class="blur-xl" />
    <div
      class="container mx-auto absolute inset-0 flex flex-col items-start justify-center bg-gradient-to-l from-white-200 md:bg-none text-neutral-100">
      <h1 style="color: whitesmoke" class="text-3xl font-bold md:mb-4 md:text-4xl lg:text-6xl">SQuArE Makerspace</h1>
      <h2 style="color: whitesmoke" class="text-lg font-bold md:mb-4 lg:text-3xl">Create prototypes, bring creative
        projects to life and realize
        ideas.</h2>
      <div style="color: whitesmoke" class="max-w-sm mb-8 text-md font-light lg:max-w-md text-balance">
        <p>Rooms for intensive work, tinkering, crafting and creating.</p>
      </div>
      <RouterLink style="color: whitesmoke" :to="{ name: 'products' }">Show Now</RouterLink>
    </div>
  </div>
</template>
