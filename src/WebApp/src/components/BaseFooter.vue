<script setup lang="ts"></script>

<template>
  <section id="footer" class="py-5">
    <UContainer>
      <LanguageSwitcher />
      <p class="text-(--ui-text-muted) text-sm">
        Copyright © {{ new Date().getFullYear() }}. All rights reserved.
      </p>
    </UContainer>
  </section>
</template>
