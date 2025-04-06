<script lang="ts" setup>
import { AsyncBaseFooter } from '@/components/AsyncBaseFooter';

</script>

<template>
    <div id="default-layout">
        <BaseHeader />

        <slot />

        <AsyncBaseFooter />
    </div>
</template>
