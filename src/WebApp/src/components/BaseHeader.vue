<script setup lang="ts">
import { computed } from "vue";
import { useRoute } from "vue-router";
import BaseMenuTrigger from "./BaseMenuTrigger.vue";
import useBasket from "@/composables/useBasket";
import useIdentity from "@/composables/useIdentity";

const route = useRoute();

const { getBasket } = useBasket();
const { login, logout, user, isAuthenticated } = useIdentity();

await getBasket();

const items = computed<NavigationMenuItem[][]>(() => [
  [
    {
      label: "Home",
      to: "/",
    },
    {
      label: "Products",
      to: "/products",
      active: route.path.startsWith("/products"),
    },
    {
      label: "Categories",
      to: "/categories",
      active: route.path.startsWith("/categories"),
    },
  ],
  isAuthenticated.value
    ? [
      {
        label: user.value.name,
        avatar: {
          src: "https://github.com/benjamincanac.png",
        },
        children: [
          {
            label: 'Orders',
            icon: 'i-lucide-user',
            to: "/user/orders",
          },
          {
            label: 'Profile',
            icon: 'i-lucide-user',
            to: "/user",
          },
          {
            label: 'Logout',
            icon: 'i-lucide-log-out',
            onClick: () => {
              logout()
            }
          }
        ]
      },
      {
        label: "Cart",
        icon: "i-lucide-shopping-cart",
        to: "/cart",
      }
    ]
    : [
      {
        label: 'Sign in',
        color: 'neutral',
        variant: 'ghost',
        icon: 'i-lucide-log-in',
        onClick: () => {
          login()
        }
      }
    ]
]);

</script>

<template>
  <header class="sticky top-0 z-40 bg-white shadow-sm shadow-light-500">
    <UContainer class="container flex items-center justify-between py-4">
      <div class="flex items-center">
        <BaseMenuTrigger class="lg:hidden" />
        <BaseLogo class="md:w-[160px]" />
      </div>
      <UNavigationMenu highlight highlight-color="warning" content-orientation="vertical" :items="items"
        class="border-(--ui-border) data-[orientation=horizontal]:w-full data-[orientation=vertical]:w-48" />
      <div class="flex justify-end items-center md:w-[160px] flex-1 ml-auto gap-4 md:gap-6">
        <!-- <ProductSearch class="hidden sm:inline-flex max-w-[320px] w-[60%]" />
        <SearchTrigger />
        <div class="flex gap-4 items-center">
          <SignInLink />
          <CartTrigger />
        </div> -->
      </div>
    </UContainer>
    <!-- <Transition name="scale-y" mode="out-in">
      <div class="container mb-3 -mt-1 sm:hidden" v-if="isShowingSearch">
        <ProductSearch class="flex w-full" />
      </div>
    </Transition> -->
  </header>

</template>
