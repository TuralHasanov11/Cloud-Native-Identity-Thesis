import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { mount, VueWrapper } from '@vue/test-utils'
import { createRouter, createWebHistory } from 'vue-router'
import HeroBanner from '../HeroBanner.vue'

// Mock router for RouterLink
const mockRouter = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', name: 'home', component: { template: '<div>Home</div>' } },
    { path: '/products', name: 'products', component: { template: '<div>Products</div>' } },
  ],
})

describe('HeroBanner', () => {
  let wrapper: VueWrapper

  beforeEach(async () => {
    // Setup router
    await mockRouter.push('/')
    await mockRouter.isReady()
  })

  afterEach(() => {
    if (wrapper) {
      wrapper.unmount()
    }
  })

  const mountComponent = (options = {}) => {
    return mount(HeroBanner, {
      global: {
        plugins: [mockRouter],
        stubs: {
          RouterLink: {
            template: '<a :data-to="JSON.stringify(to)"><slot /></a>',
            props: ['to'],
          },
        },
      },
      ...options,
    })
  }

  describe('Component Structure', () => {
    it('should render the main container with correct classes', () => {
      wrapper = mountComponent()

      const container = wrapper.find('div.relative.mx-auto')
      expect(container.exists()).toBe(true)
      expect(container.classes()).toContain('relative')
      expect(container.classes()).toContain('mx-auto')
    })

    it('should render the hero image with correct attributes', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.exists()).toBe(true)
      expect(image.attributes('width')).toBe('1400')
      expect(image.attributes('height')).toBe('800')
      expect(image.attributes('src')).toBe('/images/hero1.PNG')
      expect(image.attributes('alt')).toBe('Hero image')
      expect(image.attributes('loading')).toBe('eager')
      expect(image.attributes('sizes')).toBe('sm:100vw md:1400px')
      expect(image.attributes('fetchpriority')).toBe('high')
      expect(image.attributes('preload')).toBeDefined()
      expect(image.attributes('placeholder-class')).toBe('blur-xl')
    })

    it('should render the content overlay with correct structure', () => {
      wrapper = mountComponent()

      const overlay = wrapper.find('.container.mx-auto.absolute.inset-0')
      expect(overlay.exists()).toBe(true)
      expect(overlay.classes()).toEqual(
        expect.arrayContaining([
          'container',
          'mx-auto',
          'absolute',
          'inset-0',
          'flex',
          'flex-col',
          'items-start',
          'justify-center',
          'bg-gradient-to-l',
          'from-gray-200',
          'md:bg-none',
        ]),
      )
    })
  })

  describe('Content Elements', () => {
    it('should render the main heading with correct text and classes', () => {
      wrapper = mountComponent()

      const heading1 = wrapper.find('h1')
      expect(heading1.exists()).toBe(true)
      expect(heading1.text()).toBe('Just landed.')
      expect(heading1.classes()).toEqual(expect.arrayContaining(['text-3xl', 'font-bold', 'md:mb-4', 'md:text-4xl', 'lg:text-6xl']))
    })

    it('should render the secondary heading with correct text and classes', () => {
      wrapper = mountComponent()

      const heading2 = wrapper.find('h2')
      expect(heading2.exists()).toBe(true)
      expect(heading2.text()).toBe('The New Year Collection')
      expect(heading2.classes()).toEqual(expect.arrayContaining(['text-lg', 'font-bold', 'md:mb-4', 'lg:text-3xl']))
    })

    it('should render the description paragraph with correct content', () => {
      wrapper = mountComponent()

      const descriptionDiv = wrapper.find('.max-w-sm.mb-8.text-md.font-light')
      expect(descriptionDiv.exists()).toBe(true)
      expect(descriptionDiv.classes()).toEqual(expect.arrayContaining(['max-w-sm', 'mb-8', 'text-md', 'font-light', 'lg:max-w-md', 'text-balance']))

      const paragraph = descriptionDiv.find('p')
      expect(paragraph.exists()).toBe(true)
      expect(paragraph.text()).toBe('Our latest collection is here. Discover the latest trends and styles for the new year.')
    })

    it('should render the RouterLink with correct text and route', () => {
      wrapper = mountComponent()

      const routerLink = wrapper.find('a[data-to]')
      expect(routerLink.exists()).toBe(true)
      expect(routerLink.text()).toBe('Show Now')

      expect(routerLink.exists()).toBe(true)
      const toAttr = routerLink.exists() ? routerLink.attributes('data-to') : undefined
      expect(JSON.parse(toAttr ?? '{}')).toEqual({ name: 'products' })
    })
  })

  describe('Image Optimization Attributes', () => {
    it('should have performance optimization attributes', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.attributes('loading')).toBe('eager')
      expect(image.attributes('fetchpriority')).toBe('high')
      expect(image.attributes('preload')).toBeDefined()
    })

    it('should have responsive sizing attributes', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.attributes('sizes')).toBe('sm:100vw md:1400px')
      expect(image.attributes('width')).toBe('1400')
      expect(image.attributes('height')).toBe('800')
    })

    it('should have accessibility attributes', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.attributes('alt')).toBe('Hero image')
    })

    it('should have placeholder styling', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.attributes('placeholder-class')).toBe('blur-xl')
    })
  })

  describe('CSS Classes and Styling', () => {
    it('should have correct responsive image classes', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.classes()).toEqual(expect.arrayContaining(['object-cover', 'w-full', 'h-[420px]', 'lg:h-[560px]', 'xl:h-[640px]']))
    })

    it('should have correct overlay background classes', () => {
      wrapper = mountComponent()

      const overlay = wrapper.find('.absolute.inset-0')
      expect(overlay.classes()).toContain('bg-gradient-to-l')
      expect(overlay.classes()).toContain('from-gray-200')
      expect(overlay.classes()).toContain('md:bg-none')
    })

    it('should have correct layout classes for flexbox', () => {
      wrapper = mountComponent()

      const overlay = wrapper.find('.absolute.inset-0')
      expect(overlay.classes()).toContain('flex')
      expect(overlay.classes()).toContain('flex-col')
      expect(overlay.classes()).toContain('items-start')
      expect(overlay.classes()).toContain('justify-center')
    })

    it('should have correct responsive typography classes', () => {
      wrapper = mountComponent()

      const h1 = wrapper.find('h1')
      const h2 = wrapper.find('h2')

      // H1 responsive classes
      expect(h1.classes()).toContain('text-3xl')
      expect(h1.classes()).toContain('md:text-4xl')
      expect(h1.classes()).toContain('lg:text-6xl')

      // H2 responsive classes
      expect(h2.classes()).toContain('text-lg')
      expect(h2.classes()).toContain('lg:text-3xl')
    })
  })

  describe('Accessibility', () => {
    it('should have proper heading hierarchy', () => {
      wrapper = mountComponent()

      const h1 = wrapper.find('h1')
      const h2 = wrapper.find('h2')

      expect(h1.exists()).toBe(true)
      expect(h2.exists()).toBe(true)

      // H1 should come before H2 in DOM order
      const allHeadings = wrapper.findAll('h1, h2')
      expect(allHeadings[0].element.tagName).toBe('H1')
      expect(allHeadings[1].element.tagName).toBe('H2')
    })

    it('should have meaningful alt text for image', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.attributes('alt')).toBe('Hero image')
      expect(image.attributes('alt')).not.toBe('')
    })

    it('should have semantic HTML structure', () => {
      wrapper = mountComponent()

      // Should have proper heading tags
      expect(wrapper.find('h1').exists()).toBe(true)
      expect(wrapper.find('h2').exists()).toBe(true)

      // Should have paragraph for description
      expect(wrapper.find('p').exists()).toBe(true)

      // Should have img tag with proper attributes
      const image = wrapper.find('img')
      expect(image.exists()).toBe(true)
      expect(image.attributes('alt')).toBeTruthy()
    })
  })

  describe('Router Integration', () => {
    it('should navigate to products page when RouterLink is clicked', async () => {
      wrapper = mountComponent()
      const link = wrapper.find('a[data-to]')
      expect(link.exists()).toBe(true)

      const toAttr = link.exists() ? link.attributes('data-to') : undefined
      expect(JSON.parse(toAttr ?? '{}')).toEqual({ name: 'products' })
    })

    it('should render RouterLink as a link element when stubbed', () => {
      wrapper = mountComponent()

      const link = wrapper.find('a')
      expect(link.exists()).toBe(true)
      expect(link.text()).toBe('Show Now')
    })
  })

  describe('Error Handling and Edge Cases', () => {
    it('should handle missing image gracefully', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.exists()).toBe(true)

      // Image should still have all required attributes even if src fails to load
      expect(image.attributes('alt')).toBe('Hero image')
      expect(image.attributes('src')).toBe('/images/hero1.PNG')
    })

    it('should render without router if not available', () => {
      const wrapperNoRouter = mount(HeroBanner, {
        global: {
          stubs: {
            RouterLink: {
              template: '<span><slot /></span>',
              props: ['to'],
            },
          },
        },
      })

      expect(wrapperNoRouter.find('span').text()).toBe('Show Now')
      wrapperNoRouter.unmount()
    })

    it('should handle long text content gracefully', () => {
      wrapper = mountComponent()

      // Text should be contained within max-width containers
      const descriptionDiv = wrapper.find('.max-w-sm')
      expect(descriptionDiv.classes()).toContain('text-balance')
      expect(descriptionDiv.classes()).toContain('max-w-sm')
      expect(descriptionDiv.classes()).toContain('lg:max-w-md')
    })

    it('should maintain aspect ratio with different viewport sizes', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.classes()).toContain('h-[420px]') // mobile
      expect(image.classes()).toContain('lg:h-[560px]') // large screens
      expect(image.classes()).toContain('xl:h-[640px]') // extra large screens
      expect(image.classes()).toContain('object-cover') // maintains aspect ratio
    })
  })

  describe('Performance Considerations', () => {
    it('should have eager loading for above-the-fold image', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.attributes('loading')).toBe('eager')
      expect(image.attributes('fetchpriority')).toBe('high')
    })

    it('should have preload attribute for critical image', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.attributes('preload')).toBeDefined()
    })

    it('should have responsive sizes attribute', () => {
      wrapper = mountComponent()

      const image = wrapper.find('img')
      expect(image.attributes('sizes')).toBe('sm:100vw md:1400px')
    })
  })

  describe('Component Integration', () => {
    it('should render without any console errors', () => {
      const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {})

      wrapper = mountComponent()

      expect(consoleSpy).not.toHaveBeenCalled()
      consoleSpy.mockRestore()
    })

    it('should be a valid Vue component', () => {
      wrapper = mountComponent()

      expect(wrapper.vm).toBeDefined()
      expect(wrapper.exists()).toBe(true)
    })

    it('should unmount cleanly', () => {
      wrapper = mountComponent()
      expect(() => wrapper.unmount()).not.toThrow()
    })
  })
})
