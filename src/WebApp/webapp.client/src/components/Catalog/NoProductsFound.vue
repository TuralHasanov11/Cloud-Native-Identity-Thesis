<script setup lang="ts">
import { useI18n } from 'vue-i18n'
const { t } = useI18n()
</script>

<template>
  <div class="w-full my-16 lg:my-24">
    <div class="flex flex-col items-center justify-center w-full text-center text-pretty">
      <i class="pi pi-times-circle opacity-25 mb-10" />
      <h2 class="text-2xl font-bold">{{ t('messages.shop.noProductsFound.title') }}</h2>
      <p class="mt-4 max-w-md">
        <slot>{{ t('messages.shop.noProductsFound.subText') }}</slot>
      </p>
    </div>
  </div>
</template>
