<script setup lang="ts">
const { emptyCart } = useBasket();
</script>

<template>
    <UButton icon="material-symbols:delete-rounded" color="error"
        class="cursor-pointer top-6 right-6 md:right-8 absolute" title="Empty Cart" @click="emptyCart">
    </UButton>
</template>
