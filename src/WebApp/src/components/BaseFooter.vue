<script setup lang="ts">

</script>

<template>
  <footer id="footer" class="layout-footer py-5 container mx-auto">
    <p class="text-(--ui-text-muted) text-sm">
      Copyright © {{ new Date().getFullYear() }}. All rights reserved.
    </p>
    <LanguageSwitcher />
  </footer>
</template>