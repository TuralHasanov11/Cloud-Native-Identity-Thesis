<script setup lang="ts">
import useBasket from "@/composables/useBasket";
import useIdentity from "@/composables/useIdentity";
import type { MenuItem } from "primevue/menuitem";
import { computed, ref } from "vue";

const { getBasket } = useBasket();
const { login, logout, user, isAuthenticated } = useIdentity();

await getBasket();

const items = computed<MenuItem[]>(() => [
  {
    label: "Home",
    route: "/",
    icon: "pi pi-home"
  },
  {
    label: "Products",
    route: "/products",
    icon: "pi pi-microchip"
  },
  {
    label: "Categories",
    route: "/categories",
    icon: "pi pi-building-columns"
  },
]);

const userItems = computed<MenuItem[]>(() => [
  {
    label: user.value.name,
    avatar: {
      src: "https://github.com/benjamincanac.png",
    },
    items: [
      {
        label: 'Orders',
        icon: 'pi pi-book',
        route: "/user/orders",
      },
      {
        label: 'Profile',
        icon: 'pi pi-user',
        route: "/user",
      },
      {
        label: 'Logout',
        icon: 'pi pi-sign-out',
        command: () => {
          logout()
        }
      }
    ]
  },
  {
    label: "Cart",
    icon: "pi pi-shopping-cart",
    route: "/cart",
  }
]);

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const menu = ref<any>("menu");

const toggle = (event: MouseEvent) => {
  menu?.value?.toggle(event);
};

</script>

<template>
  <header class="layout-topbar">
    <Menubar :model="items">
      <template #start>
        <BaseLogo />
      </template>
      <template #item="{ item, props, hasSubmenu }">
        <RouterLink v-if="item.route" v-slot="{ href, navigate }" :to="item.route" custom>
          <a v-ripple :href="href" v-bind="props.action" @click="navigate">
            <span :class="item.icon" />
            <span class="ml-2">{{ item.label }}</span>
          </a>
        </RouterLink>
        <a v-else v-ripple :href="item.url" :target="item.target" v-bind="props.action">
          <span :class="item.icon" />
          <span>{{ item.label }}</span>
          <span v-if="hasSubmenu" class="pi pi-fw pi-angle-down" />
        </a>
      </template>
      <template #end>
        <div class="flex items-center gap-2">
          <InputText placeholder="Search" type="text" class="w-32 sm:w-auto" />
          <template v-if="isAuthenticated">
            <Avatar :label="user.name[0]" image="https://github.com/benjamincanac.png" aria-haspopup="true"
              aria-controls="user_menu" @click="toggle" icon="pi pi-user" class="cursor-pointer" v-ripple />
            <Menu ref="menu" id="user_menu" :model="userItems" :popup="true">
              <template #item="{ item, props }">
                <RouterLink v-if="item.route" v-slot="{ href, navigate }" :to="item.route" custom>
                  <a v-ripple :href="href" v-bind="props.action" @click="navigate">
                    <span :class="item.icon" />
                    <span class="ml-2">{{ item.label }}</span>
                  </a>
                </RouterLink>
                <a v-else v-ripple :href="item.url" :target="item.target" v-bind="props.action">
                  <span :class="item.icon" />
                  <span class="ml-2">{{ item.label }}</span>
                </a>
              </template>
            </Menu>
          </template>
          <template v-else>
            <a v-bind="login" custom>
              <span class="pi pi-sign-in" />
              <span>Sign in</span>
            </a>
          </template>
        </div>

      </template>
    </Menubar>

  </header>

</template>
