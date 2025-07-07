import { computed, reactive } from 'vue'

interface LayoutConfig {
  preset: string
  primary: string
  surface: string | null
  darkTheme: boolean
  menuMode: 'static' | 'overlay'
}

interface LayoutState {
  staticMenuDesktopInactive: boolean
  overlayMenuActive: boolean
  profileSidebarVisible: boolean
  configSidebarVisible: boolean
  staticMenuMobileActive: boolean
  menuHoverActive: boolean
  activeMenuItem: string
}

const defaultLayoutConfig: LayoutConfig = {
  preset: 'Aura',
  primary: 'emerald',
  surface: null,
  darkTheme: false,
  menuMode: 'static',
}

const layoutConfig = reactive<LayoutConfig>(defaultLayoutConfig)

const defaultLayoutState: LayoutState = {
  staticMenuDesktopInactive: false,
  overlayMenuActive: false,
  profileSidebarVisible: false,
  configSidebarVisible: false,
  staticMenuMobileActive: false,
  menuHoverActive: false,
  activeMenuItem: '',
}

const layoutState = reactive<LayoutState>(defaultLayoutState)

export function useLayout() {
  const setActiveMenuItem = (item: string) => {
    layoutState.activeMenuItem = item
  }

  const toggleMenu = () => {
    if (layoutConfig.menuMode === 'overlay') {
      layoutState.overlayMenuActive = !layoutState.overlayMenuActive
    }

    if (window.innerWidth > 991) {
      layoutState.staticMenuDesktopInactive = !layoutState.staticMenuDesktopInactive
    } else {
      layoutState.staticMenuMobileActive = !layoutState.staticMenuMobileActive
    }
  }

  const isSidebarActive = computed(() => layoutState.overlayMenuActive ?? layoutState.staticMenuMobileActive)

  const getPrimary = computed(() => layoutConfig.primary)

  const getSurface = computed(() => layoutConfig.surface)

  function reset() {
    layoutConfig.preset = defaultLayoutConfig.preset
    layoutConfig.primary = defaultLayoutConfig.primary
    layoutConfig.surface = defaultLayoutConfig.surface
    layoutConfig.darkTheme = defaultLayoutConfig.darkTheme
    layoutConfig.menuMode = defaultLayoutConfig.menuMode

    layoutState.staticMenuDesktopInactive = defaultLayoutState.staticMenuDesktopInactive
    layoutState.overlayMenuActive = defaultLayoutState.overlayMenuActive
    layoutState.profileSidebarVisible = defaultLayoutState.profileSidebarVisible
    layoutState.configSidebarVisible = defaultLayoutState.configSidebarVisible
    layoutState.staticMenuMobileActive = defaultLayoutState.staticMenuMobileActive
    layoutState.menuHoverActive = defaultLayoutState.menuHoverActive
    layoutState.activeMenuItem = defaultLayoutState.activeMenuItem
  }

  return {
    layoutConfig,
    layoutState,
    toggleMenu,
    isSidebarActive,
    getPrimary,
    getSurface,
    setActiveMenuItem,
    reset,
  }
}
