<script lang="ts" setup>
import useBasket from '@/composables/basket/useBasket'
import useIdentity from '@/composables/identity/useIdentity'
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRoute } from 'vue-router'

const { t } = useI18n()
const { user, logout } = useIdentity()

const { cart } = useBasket()
const route = useRoute()

const activeTab = computed(() => route.query.tab || 'my-details')
const showLoader = computed(() => !cart.value)
</script>

<template>
  <main id="user-orders" class="container mx-auto py-5">
    <div class="container min-h-[600px]">
      <div v-if="showLoader" class="flex flex-col min-h-[500px]">
        <LoadingIcon class="m-auto" />
      </div>
      <div v-else class="flex flex-col items-start justify-between w-full lg:gap-12 mb-24 lg:flex-row">
        <div class="mt-2 w-full lg:max-w-[260px]">
          <section class="my-8 flex gap-4 items-start justify-center w-full">
            <!-- <img v-if="avatar" :src="avatar" class="rounded-full aspect-square border border-white" alt="user-image" width="48" height="48" > -->
            <div class="flex-1 text-balance leading-tight w-full text-ellipsis overflow-hidden">
              <div class="text-lg font-semibold">Welcome, {{ user?.name }}</div>
              <span v-if="user?.email" class="text-gray-400 font-light" :title="user?.email">{{ user?.email }}</span>
            </div>
            <Button class="flex text-gray-700 items-center flex-col p-2 px-4 rounded-lg hover:bg-white hover:text-red-700 lg:hidden" @click="logout">
              <i class="pi pi-sign-out" />
              <small>{{ t('messages.account.logout') }}</small>
            </Button>
          </section>
          <hr class="my-8" />
          <nav class="flex text-gray-700 lg:grid flex-wrap w-full gap-1.5 my-8 min-w-[240px] lg:w-auto items-start">
            <RouterLink to="/user?tab=my-details" class="flex items-center gap-4 p-2 px-4" :class="{ active: activeTab == 'my-details' }">
              <i class="pi pi-user" />
              {{ t('messages.general.myDetails') }}
            </RouterLink>
            <RouterLink to="/user?tab=orders" class="flex items-center gap-4 p-2 px-4" :class="{ active: activeTab == 'orders' }">
              <i class="pi pi-shopping-bag" />
              {{ t('messages.shop.order', 2) }}
            </RouterLink>
          </nav>
          <div class="hidden lg:block">
            <hr class="my-8" />
            <Button severity="danger" @click="logout">
              <i class="pi pi-sign-out" />
              {{ t('messages.account.logout') }}
            </Button>
          </div>
        </div>

        <main class="flex-1 w-full rounded-lg max-w-screen-lg">
          <AccountMyDetails v-if="activeTab === 'my-details'" />
        </main>
      </div>
    </div>
  </main>
</template>
