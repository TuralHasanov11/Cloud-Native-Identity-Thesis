<script setup lang="ts">
import BaseError from '@/components/BaseError.vue'
import { onMounted } from 'vue'

import { useToast } from 'primevue/usetoast'

const toast = useToast()

const { error } = defineProps<{
  error: Error
}>()

function showToast() {
  toast.add({
    summary: error.name,
    detail: error.message,
  })
}

onMounted(() => {
  showToast()
})
</script>

<template>
  <div class="error">
    <BaseHeader />

    <main>
      <div class="container mx-auto">
        <BaseError :error="error" />
      </div>
    </main>

    <BaseFooter />
  </div>
</template>
