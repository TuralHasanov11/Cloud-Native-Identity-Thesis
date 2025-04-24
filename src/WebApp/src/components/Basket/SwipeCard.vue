<script setup lang="ts">
import { useSwipe } from '@vueuse/core';
import { ref } from 'vue';
const emit = defineEmits(['remove']);

const isAlive = ref(true);
const el = ref(null);
const { isSwiping, lengthX } = useSwipe(el, {
    passive: true,
    onSwipeEnd() {
        if (lengthX.value > 80) {
            isAlive.value = false;
            emit('remove');
        }
    },
});
</script>

<template>
    <div v-if="isAlive" class="rounded-lg flex h-16 w-full overflow-hidden relative items-center">
        <i class="pi pi-times-circle transform transition-all right-0 w-6 scale-0 absolute"
            :class="{ 'scale-100': lengthX > 40, 'delete-ready': lengthX > 80 }" />
        <div class="rounded-lg inset-0 absolute" :class="{ 'transition-all': !isSwiping }" ref="el"
            :style="{ transform: isSwiping ? `translateX(-${lengthX}px)` : `none` }">
            <slot />
        </div>
    </div>
</template>



<style>
.delete-ready {
    color: #ef4444;
    /* Tailwind's text-red-500 equivalent */
}
</style>