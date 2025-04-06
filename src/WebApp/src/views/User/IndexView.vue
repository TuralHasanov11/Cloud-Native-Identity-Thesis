<script lang="ts" setup>
import DefaultLayout from '@/layouts/DefaultLayout.vue'

import useBasket from "@/composables/useBasket";
import useCustomer from "@/composables/useCustomer";
import useIdentity from "@/composables/useIdentity";
import { computed } from "vue";
import { useRoute } from "vue-router";

const { user, logout } = useIdentity();

const { cart } = useBasket();
const { customer } = useCustomer();
const route = useRoute();

const activeTab = computed(() => route.query.tab || "my-details");
const showLoader = computed(() => !cart.value && !customer.value);

</script>

<template>
  <DefaultLayout>
    <main id="user-orders">
      <UContainer>
        <div class="container min-h-[600px]">
          <div v-if="showLoader" class="flex flex-col min-h-[500px]">
            <LoadingIcon class="m-auto" />
          </div>
          <div v-else class="flex flex-col items-start justify-between w-full lg:gap-12 mb-24 lg:flex-row">
            <div class="mt-2 lg:sticky top-16 w-full lg:max-w-[260px]">
              <section class="my-8 flex gap-4 items-start justify-center w-full">
                <!-- <img v-if="avatar" :src="avatar" class="rounded-full aspect-square border border-white" alt="user-image" width="48" height="48" > -->
                <div class="flex-1 text-balance leading-tight w-full text-ellipsis overflow-hidden">
                  <div class="text-lg font-semibold">
                    Welcome, {{ user?.name }}
                  </div>
                  <span v-if="user?.email" class="text-gray-400 font-light" :title="user?.email">{{ user?.email
                  }}</span>
                </div>
                <button
                  class="flex text-gray-700 items-center flex-col p-2 px-4 rounded-lg hover:bg-white hover:text-red-700 lg:hidden"
                  @click="logout">
                  <UIcon name="ion:log-out-outline" size="22" />
                  <small>{{ $t("messages.account.logout") }}</small>
                </button>
              </section>
              <hr class="my-8">
              <nav class="flex text-gray-700 lg:grid flex-wrap w-full gap-1.5 my-8 min-w-[240px] lg:w-auto items-start">
                <RouterLink to="/user?tab=my-details" class="flex items-center gap-4 p-2 px-4"
                  :class="{ active: activeTab == 'my-details' }">
                  <UIcon name="ion:information-circle-outline" size="22" />
                  {{ $t("messages.general.myDetails") }}
                </RouterLink>
                <RouterLink to="/user?tab=orders" class="flex items-center gap-4 p-2 px-4"
                  :class="{ active: activeTab == 'orders' }">
                  <UIcon name="ion:bag-check-outline" size="22" />
                  {{ $t("messages.shop.order", 2) }}
                </RouterLink>
              </nav>
              <div class="hidden lg:block">
                <hr class="my-8">
                <button
                  class="flex text-gray-700 items-center gap-4 p-2 px-4 w-full rounded-lg hover:bg-white hover:text-red-700"
                  @click="logout">
                  <UIcon name="ion:log-out-outline" size="22" />
                  {{ $t("messages.account.logout") }}
                </button>
              </div>
            </div>

            <main class="flex-1 w-full lg:my-8 rounded-lg max-w-screen-lg lg:sticky top-24">
              <AccountMyDetails v-if="activeTab === 'my-details'" />
              <OrderList v-else-if="activeTab === 'orders'" />
            </main>
          </div>
        </div>
      </UContainer>
    </main>
  </DefaultLayout>
</template>
