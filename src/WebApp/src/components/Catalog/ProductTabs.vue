<script lang="ts" setup>
import type { Product } from '@/types/catalog';
import type { TabsItem } from '@nuxt/ui';
import { computed } from 'vue';

const { product } = defineProps<{
    product: Product;
}>()

const tabs = computed<TabsItem[]>(() => [
    {
        label: 'Description',
        icon: 'i-lucide-user',
        content: product?.description,
    },
])

</script>

<template>
    <UTabs :content="false" :items="tabs" class="w-full" />
</template>