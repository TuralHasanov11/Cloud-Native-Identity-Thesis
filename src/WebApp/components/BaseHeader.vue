<script setup lang="ts">
import type { NavigationMenuItem } from "@nuxt/ui";

const route = useRoute();

const {getBasket} = useBasket();

await getBasket()

const items = computed<NavigationMenuItem[]>(() => [
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
  {
    label: "Benjamin",
    avatar: {
      src: "https://github.com/benjamincanac.png",
    },
    children:[
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
      to: "/user/logout",
      external: true,
    }
    ]
  },
  {
    label: "Cart",
    icon: "i-lucide-shopping-cart",
    to: "/cart",
  }
]);

const search = ref<string>("");

function signUp() {}

function login() {}

function onSubmitSearch() {
  console.log(search);
}
</script>

<template>
  <UHeader>
    <template #left>
      <NuxtLink to="/">
        <LogoPro class="w-auto h-6 shrink-0" />
      </NuxtLink>
      <TemplateMenu />
    </template>

    <UNavigationMenu :items="items" variant="link" />

    <template #right>
      
      <UColorModeButton />
      <UButton label="Sign in" color="neutral" variant="ghost" @click="login" />
      <UButton
        label="Sign up"
        color="neutral"
        trailing-icon="i-lucide-arrow-right"
        class="hidden lg:flex"
        @click="signUp"
      />
    </template>

    <template #body>
      <UNavigationMenu :items="items" orientation="vertical" />

      <form @submit.prevent="onSubmitSearch">
        <UFormField name="search" size="lg">
          <UInput
            v-model="search"
            type="search"
            class="w-full"
            placeholder="Search Products"
          >
            <template #trailing>
              <UButton type="submit" size="xs" color="neutral" label="Search" />
            </template>
          </UInput>
        </UFormField>
      </form>
    </template>
  </UHeader>
</template>
