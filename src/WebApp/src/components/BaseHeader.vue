<script setup lang="ts">
import useBasket from "@/composables/useBasket";
import useIdentity from "@/composables/useIdentity";
import type { NavigationMenuItem } from "@nuxt/ui";
import { computed } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();

const { getBasket } = useBasket();
const { login, logout, user } = useIdentity();

await getBasket()

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
  [
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
    },
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
  <UNavigationMenu highlight highlight-color="warning" orientation="horizontal" :items="items"
    class="data-[orientation=horizontal]:border-b border-(--ui-border) data-[orientation=horizontal]:w-full data-[orientation=vertical]:w-48" />
</template>
