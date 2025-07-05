import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest'
import { useLayout } from './useLayout'

describe('useLayout', () => {
  let originalInnerWidth: number

  beforeEach(() => {
    // Store original window.innerWidth
    originalInnerWidth = window.innerWidth

    // Reset window size to a default value
    Object.defineProperty(window, 'innerWidth', {
      writable: true,
      configurable: true,
      value: 1024,
    })

    const { reset } = useLayout()
    reset()
  })

  afterEach(() => {
    // Restore original window.innerWidth
    Object.defineProperty(window, 'innerWidth', {
      writable: true,
      configurable: true,
      value: originalInnerWidth,
    })

    vi.clearAllMocks()

    const { reset } = useLayout()
    reset()
  })

  describe('Initial State', () => {
    it('should initialize with correct default layout config', () => {
      const { layoutConfig } = useLayout()

      expect(layoutConfig.preset).toBe('Aura')
      expect(layoutConfig.primary).toBe('emerald')
      expect(layoutConfig.surface).toBeNull()
      expect(layoutConfig.darkTheme).toBe(false)
      expect(layoutConfig.menuMode).toBe('static')
    })

    it('should initialize with correct default layout state', () => {
      const { layoutState } = useLayout()

      expect(layoutState.staticMenuDesktopInactive).toBe(false)
      expect(layoutState.overlayMenuActive).toBe(false)
      expect(layoutState.profileSidebarVisible).toBe(false)
      expect(layoutState.configSidebarVisible).toBe(false)
      expect(layoutState.staticMenuMobileActive).toBe(false)
      expect(layoutState.menuHoverActive).toBe(false)
      expect(layoutState.activeMenuItem).toBe('')
    })

    it('should initialize computed properties correctly', () => {
      const { isSidebarActive, getPrimary, getSurface } = useLayout()

      expect(isSidebarActive.value).toBe(false)
      expect(getPrimary.value).toBe('emerald')
      expect(getSurface.value).toBeNull()
    })
  })

  describe('setActiveMenuItem', () => {
    it('should set active menu item correctly', () => {
      const { layoutState, setActiveMenuItem } = useLayout()

      setActiveMenuItem('dashboard')
      expect(layoutState.activeMenuItem).toBe('dashboard')
    })

    it('should handle empty string as active menu item', () => {
      const { layoutState, setActiveMenuItem } = useLayout()

      setActiveMenuItem('dashboard')
      setActiveMenuItem('')
      expect(layoutState.activeMenuItem).toBe('')
    })

    it('should handle special characters in menu item names', () => {
      const { layoutState, setActiveMenuItem } = useLayout()

      const specialMenuItem = 'menu-item_with@special#characters'
      setActiveMenuItem(specialMenuItem)
      expect(layoutState.activeMenuItem).toBe(specialMenuItem)
    })

    it('should handle very long menu item names', () => {
      const { layoutState, setActiveMenuItem } = useLayout()

      const longMenuItem = 'a'.repeat(1000)
      setActiveMenuItem(longMenuItem)
      expect(layoutState.activeMenuItem).toBe(longMenuItem)
    })
  })

  describe('toggleMenu - Overlay Mode', () => {
    it('should toggle overlay menu when in overlay mode', () => {
      const { layoutConfig, layoutState, toggleMenu } = useLayout()

      layoutConfig.menuMode = 'overlay'

      expect(layoutState.overlayMenuActive).toBe(false)

      toggleMenu()
      expect(layoutState.overlayMenuActive).toBe(true)

      toggleMenu()
      expect(layoutState.overlayMenuActive).toBe(false)
    })

    it('should handle overlay mode on desktop (width > 991)', () => {
      Object.defineProperty(window, 'innerWidth', {
        writable: true,
        configurable: true,
        value: 1200,
      })

      const { layoutConfig, layoutState, toggleMenu } = useLayout()
      layoutConfig.menuMode = 'overlay'

      toggleMenu()
      expect(layoutState.overlayMenuActive).toBe(true)
      expect(layoutState.staticMenuDesktopInactive).toBe(true)
    })

    it('should handle overlay mode on mobile (width <= 991)', () => {
      Object.defineProperty(window, 'innerWidth', {
        writable: true,
        configurable: true,
        value: 768,
      })

      const { layoutConfig, layoutState, toggleMenu } = useLayout()
      layoutConfig.menuMode = 'overlay'

      toggleMenu()
      expect(layoutState.overlayMenuActive).toBe(true)
      expect(layoutState.staticMenuMobileActive).toBe(true)
    })
  })

  describe('toggleMenu - Static Mode', () => {
    it('should toggle desktop static menu when width > 991', () => {
      Object.defineProperty(window, 'innerWidth', {
        writable: true,
        configurable: true,
        value: 1200,
      })

      const { layoutConfig, layoutState, toggleMenu } = useLayout()
      layoutConfig.menuMode = 'static'

      expect(layoutState.staticMenuDesktopInactive).toBe(false)

      toggleMenu()
      expect(layoutState.staticMenuDesktopInactive).toBe(true)
      expect(layoutState.overlayMenuActive).toBe(false)

      toggleMenu()
      expect(layoutState.staticMenuDesktopInactive).toBe(false)
    })

    it('should toggle mobile static menu when width <= 991', () => {
      Object.defineProperty(window, 'innerWidth', {
        writable: true,
        configurable: true,
        value: 768,
      })

      const { layoutConfig, layoutState, toggleMenu } = useLayout()
      layoutConfig.menuMode = 'static'

      expect(layoutState.staticMenuMobileActive).toBe(false)

      toggleMenu()
      expect(layoutState.staticMenuMobileActive).toBe(true)
      expect(layoutState.overlayMenuActive).toBe(false)

      toggleMenu()
      expect(layoutState.staticMenuMobileActive).toBe(false)
    })
  })

  describe('toggleMenu - Boundary Cases', () => {
    it('should handle exact boundary width of 991', () => {
      Object.defineProperty(window, 'innerWidth', {
        writable: true,
        configurable: true,
        value: 991,
      })

      const { layoutState, toggleMenu } = useLayout()

      toggleMenu()
      expect(layoutState.staticMenuMobileActive).toBe(false)
      expect(layoutState.staticMenuDesktopInactive).toBe(true)
    })

    it('should handle width of 992 (just above boundary)', () => {
      Object.defineProperty(window, 'innerWidth', {
        writable: true,
        configurable: true,
        value: 992,
      })

      const { layoutState, toggleMenu } = useLayout()

      toggleMenu()
      expect(layoutState.staticMenuDesktopInactive).toBe(true)
      expect(layoutState.staticMenuMobileActive).toBe(false)
    })

    it('should handle very small screen widths', () => {
      Object.defineProperty(window, 'innerWidth', {
        writable: true,
        configurable: true,
        value: 320,
      })

      const { layoutState, toggleMenu } = useLayout()

      toggleMenu()
      expect(layoutState.staticMenuMobileActive).toBe(true)
    })

    it('should handle very large screen widths', () => {
      Object.defineProperty(window, 'innerWidth', {
        writable: true,
        configurable: true,
        value: 4000,
      })

      const { layoutState, toggleMenu } = useLayout()

      toggleMenu()
      expect(layoutState.staticMenuDesktopInactive).toBe(true)
    })
  })

  describe('isSidebarActive Computed Property', () => {
    it('should return true when overlayMenuActive is true', () => {
      const { layoutState, isSidebarActive } = useLayout()

      layoutState.overlayMenuActive = true
      expect(isSidebarActive.value).toBe(true)
    })

    it('should return true when staticMenuMobileActive is true', () => {
      const { layoutState, isSidebarActive } = useLayout()

      layoutState.staticMenuMobileActive = true
      expect(isSidebarActive.value).toBe(true)
    })

    it('should return true when both overlay and mobile menus are active', () => {
      const { layoutState, isSidebarActive } = useLayout()

      layoutState.overlayMenuActive = true
      layoutState.staticMenuMobileActive = true
      expect(isSidebarActive.value).toBe(true)
    })

    it('should return false when both overlay and mobile menus are inactive', () => {
      const { layoutState, isSidebarActive } = useLayout()

      layoutState.overlayMenuActive = false
      layoutState.staticMenuMobileActive = false
      expect(isSidebarActive.value).toBe(false)
    })

    it('should be reactive to state changes', () => {
      const { layoutState, isSidebarActive } = useLayout()

      expect(isSidebarActive.value).toBe(false)

      layoutState.overlayMenuActive = true
      expect(isSidebarActive.value).toBe(true)

      layoutState.overlayMenuActive = false
      layoutState.staticMenuMobileActive = true
      expect(isSidebarActive.value).toBe(true)

      layoutState.staticMenuMobileActive = false
      expect(isSidebarActive.value).toBe(false)
    })
  })

  describe('getPrimary Computed Property', () => {
    it('should return current primary color', () => {
      const { getPrimary } = useLayout()

      expect(getPrimary.value).toBe('emerald')
    })

    it('should be reactive to config changes', () => {
      const { layoutConfig, getPrimary } = useLayout()

      layoutConfig.primary = 'blue'
      expect(getPrimary.value).toBe('blue')

      layoutConfig.primary = 'red'
      expect(getPrimary.value).toBe('red')
    })

    it('should handle empty string primary color', () => {
      const { layoutConfig, getPrimary } = useLayout()

      layoutConfig.primary = ''
      expect(getPrimary.value).toBe('')
    })
  })

  describe('getSurface Computed Property', () => {
    it('should return current surface value', () => {
      const { getSurface } = useLayout()

      expect(getSurface.value).toBeNull()
    })

    it('should be reactive to config changes', () => {
      const { layoutConfig, getSurface } = useLayout()

      layoutConfig.surface = 'dark'
      expect(getSurface.value).toBe('dark')

      layoutConfig.surface = 'light'
      expect(getSurface.value).toBe('light')

      layoutConfig.surface = null
      expect(getSurface.value).toBeNull()
    })
  })

  describe('Reactive State Management', () => {
    it('should maintain state across multiple composable instances', () => {
      const layout1 = useLayout()
      const layout2 = useLayout()

      layout1.layoutState.activeMenuItem = 'test-menu'
      expect(layout2.layoutState.activeMenuItem).toBe('test-menu')

      layout2.layoutConfig.primary = 'purple'
      expect(layout1.layoutConfig.primary).toBe('purple')
    })

    it('should handle concurrent state modifications', () => {
      const { layoutState, setActiveMenuItem, toggleMenu } = useLayout()

      setActiveMenuItem('menu1')
      toggleMenu()

      expect(layoutState.activeMenuItem).toBe('menu1')
      expect(layoutState.staticMenuDesktopInactive).toBe(true)
    })
  })

  describe('Edge Cases and Error Handling', () => {
    it('should handle undefined window.innerWidth', () => {
      const originalDescriptor = Object.getOwnPropertyDescriptor(window, 'innerWidth')

      Object.defineProperty(window, 'innerWidth', {
        get: () => undefined,
        configurable: true,
      })

      const { toggleMenu } = useLayout()

      // Should not throw error
      expect(() => toggleMenu()).not.toThrow()

      // Restore original descriptor
      if (originalDescriptor) {
        Object.defineProperty(window, 'innerWidth', originalDescriptor)
      }
    })

    it('should handle invalid menu mode', () => {
      const { layoutConfig, layoutState, toggleMenu } = useLayout()

      layoutConfig.menuMode = 'invalid-mode' as never

      // Should still handle desktop/mobile logic
      Object.defineProperty(window, 'innerWidth', {
        writable: true,
        configurable: true,
        value: 1200,
      })

      toggleMenu()
      expect(layoutState.staticMenuDesktopInactive).toBe(true)
      expect(layoutState.overlayMenuActive).toBe(false)
    })

    it('should handle rapid successive toggleMenu calls', () => {
      const { layoutState, toggleMenu } = useLayout()

      toggleMenu()
      toggleMenu()
      toggleMenu()

      expect(layoutState.staticMenuDesktopInactive).toBe(true)
    })
  })

  describe('Configuration Persistence', () => {
    it('should maintain layout configuration across state changes', () => {
      const { layoutConfig, toggleMenu } = useLayout()

      const originalConfig = { ...layoutConfig }

      toggleMenu()

      expect(layoutConfig.preset).toBe(originalConfig.preset)
      expect(layoutConfig.primary).toBe(originalConfig.primary)
      expect(layoutConfig.surface).toBe(originalConfig.surface)
      expect(layoutConfig.darkTheme).toBe(originalConfig.darkTheme)
      expect(layoutConfig.menuMode).toBe(originalConfig.menuMode)
    })
  })
})
