<script setup lang="ts">
import { useHelpers } from '@/composables/useHelpers';

const { toggleMobileMenu } = useHelpers();
</script>

<template>
    <div>
        <span class="menu-outline mr-4 cursor-pointer" @click="toggleMobileMenu()"></span>
    </div>
</template>
