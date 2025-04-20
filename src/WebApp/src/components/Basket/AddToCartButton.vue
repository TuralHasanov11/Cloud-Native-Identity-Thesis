<script setup lang="ts">
import useBasket from '@/composables/useBasket';
import { computed, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';

const { isUpdatingCart } = useBasket();
defineProps<{
  disabled: boolean;
}>();
const isLoading = ref(false);
const { t } = useI18n();
const addToCartButtonText = computed(() => (isLoading.value ? t('messages.shop.adding') : t('messages.shop.addToCart')));

watch(isUpdatingCart, () => {
  if (isUpdatingCart.value) {
    isLoading.value = true;
  } else {
    isLoading.value = false;
  }
})

</script>

<template>
  <UButton type="submit"
    class="rounded-lg flex font-bold bg-gray-800 text-white text-center min-w-[150px] p-2.5 gap-4 items-center justify-center focus:outline-none"
    :disabled="disabled" @click="isLoading = true">
    <span>{{ addToCartButtonText }}</span>
  </UButton>
</template>

<style scoped>
button {
  outline: none !important;
  transition: all 150ms ease-in;
}
</style>
