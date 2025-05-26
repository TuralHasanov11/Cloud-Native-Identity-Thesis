<script lang="ts" setup>
import { AsyncBaseFooter } from '@/components/AsyncBaseFooter';
import { AsyncCartSummary } from '@/components/Basket/AsyncCartSummary';
import useBasket from '@/composables/useBasket';

const { isShowingCart } = useBasket();

</script>

<template>
    <div id="default-layout">
        <BaseHeader />

        <div class="layout-main-container">
            <RouterView />
        </div>

        <Transition name="slide-from-right">
            <AsyncCartSummary v-if="isShowingCart" />
        </Transition>

        <AsyncBaseFooter />
    </div>
</template>
