<script lang="ts" setup>
import type { Product } from '@/types/catalog';
import { computed } from 'vue';

const { product } = defineProps<{
    product: Product;
}>()

const tabs = computed(() => [
    {
        label: 'Description',
        icon: 'i-lucide-user',
        content: product?.description,
    },
])

</script>

<template>
    <UTabs :unmount-on-hide="false" :items="tabs" class="w-full" />
</template>