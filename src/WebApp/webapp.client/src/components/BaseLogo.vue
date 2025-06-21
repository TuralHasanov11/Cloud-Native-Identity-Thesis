<script setup lang="ts">
const logoUrl = '/images/logo.jpg'
const faviconUrl = '/favicon.ico'
</script>

<template>
  <RouterLink to="/" class="inline-flex items-center gap-2">
    <img
      v-if="logoUrl"
      :src="logoUrl"
      width="32"
      height="32"
      alt="Logo"
      class="object-contain h-10"
    />
    <div v-else class="flex items-center gap-2 text-lg font-bold">
      <img :src="faviconUrl" alt="Logo" width="32" height="32" />
      <span>E-Commerce</span>
    </div>
  </RouterLink>
</template>
