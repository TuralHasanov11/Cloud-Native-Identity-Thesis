<script setup lang="ts">
import { onErrorCaptured, ref } from 'vue';
import { RouterView } from 'vue-router'
import BaseError from '@/components/BaseError.vue';

const error = ref<Error | null>(null);

onErrorCaptured((error) => {
  console.error('Error captured in App.vue:', error);
  return false; // Prevents the error from propagating further
});

</script>

<template>

  <Suspense>
    <div v-if="!error" class="app">
      <UApp>
        <RouterView />
      </UApp>
    </div>
    <div v-else>
      <BaseError :error="error" />
    </div>

    <template #fallback>
      <UApp>
        Loading
      </UApp>
    </template>
  </Suspense>
</template>
