<script setup lang="ts">
const { toggleMobileMenu } = useHelpers();
</script>

<template>
    <div>
        <UIcon name="ion:menu-outline" size="26" class="mr-4 cursor-pointer" @click="toggleMobileMenu" />
    </div>
</template>
