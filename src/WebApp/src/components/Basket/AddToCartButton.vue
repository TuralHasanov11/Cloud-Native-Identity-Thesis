<script setup lang="ts">
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
    :class="{ disabled: 'cursor-not-allowed bg-gray-400' }" :disabled="disabled" @click="isLoading = true">
    <span>{{ addToCartButtonText }}</span>
    <LoadingIcon v-if="isLoading" stroke="4" size="12" color="#fff" />
  </UButton>
</template>

<style scoped>
button {
  outline: none !important;
  transition: all 150ms ease-in;
}
</style>
