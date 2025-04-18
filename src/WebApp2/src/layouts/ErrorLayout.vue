<script setup lang="ts">
import BaseError from '@/components/BaseError.vue';
import { onMounted } from 'vue';

const toast = useToast()

const { error } = defineProps<{
    error: Error;
}>();

function showToast() {
    toast.add({
        title: error.name,
        description: error.message,
    })
}

onMounted(() => {
    showToast()
})

</script>

<template>
    <div class="error">
        <BaseHeader />

        <UMain>
            <UContainer>
                <UPage>
                    <BaseError :error="error" />
                </UPage>
            </UContainer>
        </UMain>

        <BaseFooter />
    </div>
</template>