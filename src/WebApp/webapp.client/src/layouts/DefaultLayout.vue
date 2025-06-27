<script lang="ts" setup>
import { AsyncBaseFooter } from '@/components/AsyncBaseFooter'
import { AsyncCartSummary } from '@/components/Basket/AsyncCartSummary'
import useBasket from '@/composables/useBasket'

const { isShowingCart } = useBasket()
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

<style scoped lang="css">
.layout-wrapper {
  min-height: 100vh;
}

.layout-main-container {
  min-height: 100vh;
  transition: margin-left var(--layout-section-transition-duration);
}

.layout-topbar {
  position: fixed;
  z-index: 997;
  left: 0;
  top: 0;
  width: 100%;
  background-color: var(--surface-card);
  transition: left var(--layout-section-transition-duration);

  .layout-topbar-menu-button {
    display: none;
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
    }

    .layout-topbar-menu-content {
      flex-direction: column;
    }
  }
}

/* Header */

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

      &:focus {
        outline-offset: -1px;
        box-shadow: inset var(--focus-ring-shadow);
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
  .landing-wrapper {
    width: 1504px;
    margin-left: auto !important;
    margin-right: auto !important;
  }
}

@media (min-width: 992px) {
  .layout-wrapper {
    &.layout-overlay {
      &.layout-overlay-active {}
    }

    &.layout-static {
      &.layout-static-inactive {}
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
      .layout-mask {
        display: block;
      }
    }
  }
}

.layout-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem 0 1rem 0;
  gap: 0.5rem;
  border-top: 1px solid var(--surface-border);
}
</style>
