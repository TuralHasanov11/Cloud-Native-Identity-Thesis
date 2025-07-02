<script setup lang="ts">
import { useI18n } from 'vue-i18n'
const { t } = useI18n()
defineProps<{
  productCount: number
}>()
</script>

<template>
  <div v-if="productCount > 0" class="text-sm font-light">
    <span>{{ t('messages.shop.productResultCount.showing') + ' ' }}</span>
    (<span>{{ t('messages.shop.productResultCount.of') }}</span> <span class="font-normal">{{ productCount }}</span
    >)
  </div>
</template>
