<script setup lang="ts">
import { useToast } from 'primevue'
import { onErrorCaptured, ref } from 'vue'
import { RouterView } from 'vue-router'

const toast = useToast()
const error = ref<Error | null>(null)

onErrorCaptured((error) => {
  console.error('Error captured in App.vue:', error)
  toast.add({
    severity: 'error',
    summary: 'An error occurred',
    detail: error.message,
    life: 5000,
  })
  return false // Prevents the error from propagating further
})
</script>

<template>
  <Suspense>
    <div v-if="!error" class="app">
      <RouterView />
    </div>
    <div v-else>
      <BaseError :error="error" />
    </div>

    <template #fallback> Loading </template>
  </Suspense>
</template>
