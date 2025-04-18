<script setup lang="ts">

defineProps<{
    error: Error;
}>();

</script>

<template>
    <UAlert color="error" :title="error.name" :description="error.message" icon="i-lucide-terminal" />
</template>
