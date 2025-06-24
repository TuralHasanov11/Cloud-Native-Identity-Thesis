<script setup lang="ts">
import type { BaseMenuItemProp } from '@/types/base';
import { ref } from 'vue';

const model = ref<BaseMenuItemProp[]>([
  {
    label: 'Dashboard',
    icon: 'pi pi-fw pi-home',
    to: '/'
  },
  {
    label: 'Catalog',
    icon: 'pi pi-fw pi-home',
    items: [
      { label: 'Products', icon: 'pi pi-fw pi-id-card', to: '/admin/catalog/products' },
      { label: 'Create Product', icon: 'pi pi-fw pi-id-card', to: '/admin/catalog/products/create' },
      { label: 'Brands', icon: 'pi pi-fw pi-id-card', to: '/admin/catalog/brands' },
    ],
  },
  {
    label: 'Orders',
    icon: 'pi pi-fw pi-briefcase',
    to: '/admin/orders',
  },
]);
</script>

<template>
  <ul class="layout-menu">
    <template v-for="(item, i) in model" :key="item.url">
      <AdminMenuItem v-if="!item?.separator" :item="item" :index="i" />
      <li v-if="item?.separator" class="menu-separator"></li>
    </template>
  </ul>
</template>
