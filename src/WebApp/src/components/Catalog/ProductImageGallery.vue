<script lang="ts" setup>
import type { Product } from '@/types/catalog';

defineProps<{
    mainImage: string;
    gallery: string[];
    product: Product;
}>()
</script>

<template>
    <UCarousel v-slot="{ item }" loop arrows dots :autoplay="{ delay: 2000 }" :items="gallery"
        :ui="{ item: 'basis-1/3' }">
        <img :src="item" width="234" height="234" class="rounded-lg">
    </UCarousel>
</template>