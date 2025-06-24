<script setup lang="ts">
import { useLayout } from '@/composables/useLayout';
import type { AdminMenuItemProp } from '@/types/admin/common';
import { onBeforeMount, ref, watch } from 'vue';
import { useRoute } from 'vue-router';

interface Props {
  item: AdminMenuItemProp;
  index?: number;
  root?: boolean;
  parentItemKey?: string | null;
}

const route = useRoute();

const { layoutState, setActiveMenuItem, toggleMenu } = useLayout();


const props = defineProps<Props>();

const isActiveMenu = ref(false);
const itemKey = ref<string>("");

onBeforeMount(() => {
  itemKey.value = props.parentItemKey ? props.parentItemKey + '-' + props.index : String(props.index);

  const activeItem = layoutState.activeMenuItem;

  isActiveMenu.value = activeItem === itemKey.value || activeItem ? activeItem.startsWith(itemKey.value + '-') : false;
});

watch(
  () => layoutState.activeMenuItem,
  (newVal) => {
    isActiveMenu.value = newVal === itemKey.value || newVal.startsWith(itemKey.value + '-');
  }
);

function itemClick(event: MouseEvent, item: AdminMenuItemProp) {
  if (item.disabled) {
    event.preventDefault();
    return;
  }

  if ((item.to || item.url) && (layoutState.staticMenuMobileActive || layoutState.overlayMenuActive)) {
    toggleMenu();
  }

  if (item.command) {
    item.command({ originalEvent: event, item: item });
  }

  let foundItemKey: string;
  if (item.items) {
    foundItemKey = isActiveMenu.value ? (props.parentItemKey ?? "") : itemKey.value;
  } else {
    foundItemKey = itemKey.value;
  }

  setActiveMenuItem(foundItemKey);
}

function checkActiveRoute(item: AdminMenuItemProp) {
  return route.path === item.to;
}
</script>

<template>
  <li :class="{ 'layout-root-menuitem': root, 'active-menuitem': isActiveMenu }">
    <div v-if="root && item.visible !== false" class="layout-menuitem-root-text">{{ item.label }}</div>
    <a v-if="(!item.to || item.items) && item.visible !== false" :href="item.url" @click="itemClick($event, item)"
      :class="item.class" :target="item.target" tabindex="0">
      <i :class="item.icon" class="layout-menuitem-icon"></i>
      <span class="layout-menuitem-text">{{ item.label }}</span>
      <i class="pi pi-fw pi-angle-down layout-submenu-toggler" v-if="item.items"></i>
    </a>
    <RouterLink v-if="item.to && !item.items && item.visible !== false" @click="itemClick($event, item)"
      :class="[item.class, { 'active-route': checkActiveRoute(item) }]" tabindex="0" :to="item.to">
      <i :class="item.icon" class="layout-menuitem-icon"></i>
      <span class="layout-menuitem-text">{{ item.label }}</span>
      <i class="pi pi-fw pi-angle-down layout-submenu-toggler" v-if="item.items"></i>
    </RouterLink>
    <Transition v-if="item.items && item.visible !== false" name="layout-submenu">
      <ul v-show="root ? true : isActiveMenu" class="layout-submenu">
        <AdminMenuItem v-for="(child, i) in item.items" :key="child.url" :index="i" :item="child"
          :parentItemKey="itemKey" :root="false"></AdminMenuItem>
      </ul>
    </Transition>
  </li>
</template>
