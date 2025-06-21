<script setup lang="ts">
import { useLayout } from '@/composables/useLayout';
import { computed, ref, watch } from 'vue';

const { layoutConfig, layoutState, isSidebarActive } = useLayout();

const outsideClickListener = ref<
  ((event: MouseEvent) => void) | null
>(null);

watch(isSidebarActive, (newVal) => {
  if (newVal) {
    bindOutsideClickListener();
  } else {
    unbindOutsideClickListener();
  }
});

const containerClass = computed(() => {
  return {
    'layout-overlay': layoutConfig.menuMode === 'overlay',
    'layout-static': layoutConfig.menuMode === 'static',
    'layout-static-inactive': layoutState.staticMenuDesktopInactive && layoutConfig.menuMode === 'static',
    'layout-overlay-active': layoutState.overlayMenuActive,
    'layout-mobile-active': layoutState.staticMenuMobileActive
  };
});

function bindOutsideClickListener() {
  if (!outsideClickListener.value) {
    outsideClickListener.value = (event: MouseEvent) => {
      if (isOutsideClicked(event)) {
        layoutState.overlayMenuActive = false;
        layoutState.staticMenuMobileActive = false;
        layoutState.menuHoverActive = false;
      }
    };
    document.addEventListener('click', outsideClickListener.value);
  }
}

function unbindOutsideClickListener() {
  if (outsideClickListener.value) {
    document.removeEventListener('click', outsideClickListener.value);
    outsideClickListener.value = null;
  }
}

function isOutsideClicked(event: MouseEvent): boolean {
  const sidebarEl = document.querySelector('.layout-sidebar');
  const topbarEl = document.querySelector('.layout-menu-button');

  return !(sidebarEl?.isSameNode(event.target as Node) || sidebarEl?.contains(event.target as Node) || topbarEl?.isSameNode(event.target as Node) || topbarEl?.contains(event.target as Node));
}
</script>

<template>
  <div class="layout-wrapper" :class="containerClass">
    <AdminTopbar />
    <AdminSidebar />
    <div class="layout-main-container">
      <div class="layout-main">
        <RouterView />
      </div>
      <AdminFooter />
    </div>
    <div class="layout-mask animate-fadein"></div>
  </div>
  <Toast />
</template>

<style>
/* Topbar */
.layout-topbar {
  position: fixed;
  height: 4rem;
  z-index: 997;
  left: 0;
  top: 0;
  width: 100%;
  padding: 0 2rem;
  background-color: var(--surface-card);
  transition: left var(--layout-section-transition-duration);
  display: flex;
  align-items: center;

  .layout-topbar-logo-container {
    width: 20rem;
    display: flex;
    align-items: center;
  }

  .layout-topbar-logo {
    display: inline-flex;
    align-items: center;
    font-size: 1.5rem;
    border-radius: var(--content-border-radius);
    color: var(--text-color);
    font-weight: 500;
    gap: 0.5rem;

    svg {
      width: 3rem;
    }
  }

  .layout-topbar-action {
    display: inline-flex;
    justify-content: center;
    align-items: center;
    border-radius: 50%;
    width: 2.5rem;
    height: 2.5rem;
    color: var(--text-color);
    transition: background-color var(--element-transition-duration);
    cursor: pointer;

    &:hover {
      background-color: var(--surface-hover);
    }

    i {
      font-size: 1.25rem;
    }

    span {
      font-size: 1rem;
      display: none;
    }

    &.layout-topbar-action-highlight {
      background-color: var(--primary-color);
      color: var(--primary-contrast-color);
    }
  }

  .layout-menu-button {
    margin-right: 0.5rem;
  }

  .layout-topbar-menu-button {
    display: none;
  }

  .layout-topbar-actions {
    margin-left: auto;
    display: flex;
    gap: 1rem;
  }

  .layout-topbar-menu-content {
    display: flex;
    gap: 1rem;
  }

  .layout-config-menu {
    display: flex;
    gap: 1rem;
  }
}

@media (max-width: 991px) {
  .layout-topbar {
    padding: 0 2rem;

    .layout-topbar-logo-container {
      width: auto;
    }

    .layout-menu-button {
      margin-left: 0;
      margin-right: 0.5rem;
    }

    .layout-topbar-menu-button {
      display: inline-flex;
    }

    .layout-topbar-menu {
      position: absolute;
      background-color: var(--surface-overlay);
      transform-origin: top;
      box-shadow:
        0px 3px 5px rgba(0, 0, 0, 0.02),
        0px 0px 2px rgba(0, 0, 0, 0.05),
        0px 1px 4px rgba(0, 0, 0, 0.08);
      border-radius: var(--content-border-radius);
      padding: 1rem;
      right: 2rem;
      top: 4rem;
      min-width: 15rem;
      border: 1px solid var(--surface-border);

      .layout-topbar-menu-content {
        gap: 0.5rem;
      }

      .layout-topbar-action {
        display: flex;
        width: 100%;
        height: auto;
        justify-content: flex-start;
        border-radius: var(--content-border-radius);
        padding: 0.5rem 1rem;

        i {
          font-size: 1rem;
          margin-right: 0.5rem;
        }

        span {
          font-weight: medium;
          display: block;
        }
      }
    }

    .layout-topbar-menu-content {
      flex-direction: column;
    }
  }
}

/* Main */
.layout-main-container {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  justify-content: space-between;
  padding: 6rem 2rem 0 2rem;
  transition: margin-left var(--layout-section-transition-duration);
}

.layout-main {
  flex: 1 1 auto;
  padding-bottom: 2rem;
}

/* Menu */
.layout-sidebar {
  position: fixed;
  width: 20rem;
  height: calc(100vh - 8rem);
  z-index: 999;
  overflow-y: auto;
  user-select: none;
  top: 6rem;
  left: 2rem;
  transition:
    transform var(--layout-section-transition-duration),
    left var(--layout-section-transition-duration);
  background-color: var(--surface-overlay);
  border-radius: var(--content-border-radius);
  padding: 0.5rem 1.5rem;
}

.layout-menu {
  margin: 0;
  padding: 0;
  list-style-type: none;

  .layout-root-menuitem {
    >.layout-menuitem-root-text {
      font-size: 0.857rem;
      text-transform: uppercase;
      font-weight: 700;
      color: var(--text-color);
      margin: 0.75rem 0;
    }

    >a {
      display: none;
    }
  }

  a {
    user-select: none;

    &.active-menuitem {
      >.layout-submenu-toggler {
        transform: rotate(-180deg);
      }
    }
  }

  li.active-menuitem {
    >a {
      .layout-submenu-toggler {
        transform: rotate(-180deg);
      }
    }
  }

  ul {
    margin: 0;
    padding: 0;
    list-style-type: none;

    a {
      display: flex;
      align-items: center;
      position: relative;
      outline: 0 none;
      color: var(--text-color);
      cursor: pointer;
      padding: 0.75rem 1rem;
      border-radius: var(--content-border-radius);
      transition:
        background-color var(--element-transition-duration),
        box-shadow var(--element-transition-duration);

      .layout-menuitem-icon {
        margin-right: 0.5rem;
      }

      .layout-submenu-toggler {
        font-size: 75%;
        margin-left: auto;
        transition: transform var(--element-transition-duration);
      }

      &.active-route {
        font-weight: 700;
        color: var(--primary-color);
      }

      &:hover {
        background-color: var(--surface-hover);
      }
    }

    ul {
      overflow: hidden;
      border-radius: var(--content-border-radius);

      li {
        a {
          margin-left: 1rem;
        }

        li {
          a {
            margin-left: 2rem;
          }

          li {
            a {
              margin-left: 2.5rem;
            }

            li {
              a {
                margin-left: 3rem;
              }

              li {
                a {
                  margin-left: 3.5rem;
                }

                li {
                  a {
                    margin-left: 4rem;
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}

.layout-submenu-enter-from,
.layout-submenu-leave-to {
  max-height: 0;
}

.layout-submenu-enter-to,
.layout-submenu-leave-from {
  max-height: 1000px;
}

.layout-submenu-leave-active {
  overflow: hidden;
  transition: max-height 0.45s cubic-bezier(0, 1, 0, 1);
}

.layout-submenu-enter-active {
  overflow: hidden;
  transition: max-height 1s ease-in-out;
}

/* Responsive */
@media screen and (min-width: 1960px) {

  .layout-main,
  .landing-wrapper {
    width: 1504px;
    margin-left: auto !important;
    margin-right: auto !important;
  }
}

@media (min-width: 992px) {
  .layout-wrapper {
    &.layout-overlay {
      .layout-main-container {
        margin-left: 0;
        padding-left: 2rem;
      }

      .layout-sidebar {
        transform: translateX(-100%);
        left: 0;
        top: 0;
        height: 100vh;
        border-top-left-radius: 0;
        border-bottom-left-radius: 0;
        border-right: 1px solid var(--surface-border);
        transition:
          transform 0.4s cubic-bezier(0.05, 0.74, 0.2, 0.99),
          left 0.4s cubic-bezier(0.05, 0.74, 0.2, 0.99);
        box-shadow:
          0px 3px 5px rgba(0, 0, 0, 0.02),
          0px 0px 2px rgba(0, 0, 0, 0.05),
          0px 1px 4px rgba(0, 0, 0, 0.08);
      }

      &.layout-overlay-active {
        .layout-sidebar {
          transform: translateX(0);
        }
      }
    }

    &.layout-static {
      .layout-main-container {
        margin-left: 22rem;
      }

      &.layout-static-inactive {
        .layout-sidebar {
          transform: translateX(-100%);
          left: 0;
        }

        .layout-main-container {
          margin-left: 0;
          padding-left: 2rem;
        }
      }
    }

    .layout-mask {
      display: none;
    }
  }
}

@media (max-width: 991px) {
  .blocked-scroll {
    overflow: hidden;
  }

  .layout-wrapper {
    .layout-main-container {
      margin-left: 0;
      padding-left: 2rem;
    }

    .layout-sidebar {
      transform: translateX(-100%);
      left: 0;
      top: 0;
      height: 100vh;
      border-top-left-radius: 0;
      border-bottom-left-radius: 0;
      transition:
        transform 0.4s cubic-bezier(0.05, 0.74, 0.2, 0.99),
        left 0.4s cubic-bezier(0.05, 0.74, 0.2, 0.99);
    }

    .layout-mask {
      display: none;
      position: fixed;
      top: 0;
      left: 0;
      z-index: 998;
      width: 100%;
      height: 100%;
      background-color: var(--maskbg);
    }

    &.layout-mobile-active {
      .layout-sidebar {
        transform: translateX(0);
      }

      .layout-mask {
        display: block;
      }
    }
  }
}
</style>
