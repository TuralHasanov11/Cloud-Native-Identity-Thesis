<script setup lang="ts">
import { useToast } from 'primevue'
import { onMounted } from 'vue'

const { error } = defineProps<{
  error: Error
}>()

const toast = useToast()

onMounted(() => {
  toast.add({
    severity: 'error',
    summary: error.name,
    detail: error.message,
    life: 10000,
  })
})
</script>

<template>
  <Toast />
</template>
