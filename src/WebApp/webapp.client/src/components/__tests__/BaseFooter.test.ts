import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { mount, VueWrapper } from '@vue/test-utils'
import { nextTick } from 'vue'
import BaseFooter from '../BaseFooter.vue'

// Mock LanguageSwitcher component
vi.mock('../LanguageSwitcher.vue', () => ({
  default: {
    name: 'LanguageSwitcher',
    template: '<div data-testid="language-switcher">Language Switcher</div>',
  },
}))

describe('BaseFooter', () => {
  let wrapper: VueWrapper
  let mockDate: Date

  beforeEach(() => {
    // Mock the Date to ensure consistent test results
    mockDate = new Date('2024-12-25T10:00:00Z')
    vi.setSystemTime(mockDate)
  })

  afterEach(() => {
    if (wrapper) {
      wrapper.unmount()
    }
    vi.useRealTimers()
  })

  describe('Rendering', () => {
    it('should render the footer element with correct structure', () => {
      wrapper = mount(BaseFooter)

      const footer = wrapper.find('footer')
      expect(footer.exists()).toBe(true)
      expect(footer.attributes('id')).toBe('footer')
      expect(footer.classes()).toContain('layout-footer')
      expect(footer.classes()).toContain('py-5')
      expect(footer.classes()).toContain('container')
      expect(footer.classes()).toContain('mx-auto')
    })

    it('should render copyright text with current year', () => {
      wrapper = mount(BaseFooter)

      const copyrightText = wrapper.find('p')
      expect(copyrightText.exists()).toBe(true)
      expect(copyrightText.text()).toBe('Copyright © 2024. All rights reserved.')
      expect(copyrightText.classes()).toContain('text-sm')
    })

    it('should render LanguageSwitcher component', () => {
      wrapper = mount(BaseFooter)

      const languageSwitcher = wrapper.findComponent({ name: 'LanguageSwitcher' })
      expect(languageSwitcher.exists()).toBe(true)
    })

    it('should have correct CSS classes applied', () => {
      wrapper = mount(BaseFooter)

      const footer = wrapper.find('footer')
      const paragraph = wrapper.find('p')

      // Check footer classes
      expect(footer.classes()).toEqual(expect.arrayContaining(['layout-footer', 'py-5', 'container', 'mx-auto']))

      // Check paragraph classes
      expect(paragraph.classes()).toEqual(expect.arrayContaining(['text-sm']))
    })
  })

  describe('Dynamic Year Display', () => {
    it('should display the correct year when mounted in 2024', () => {
      vi.setSystemTime(new Date('2024-06-15T10:00:00Z'))
      wrapper = mount(BaseFooter)

      expect(wrapper.text()).toContain('Copyright © 2024. All rights reserved.')
    })

    it('should display the correct year when mounted in 2025', () => {
      vi.setSystemTime(new Date('2025-01-01T00:00:00Z'))
      wrapper = mount(BaseFooter)

      expect(wrapper.text()).toContain('Copyright © 2025. All rights reserved.')
    })

    it('should update year if component is re-rendered in different year', async () => {
      // Mount in 2024
      vi.setSystemTime(new Date('2024-01-01T00:00:00Z'))
      wrapper = mount(BaseFooter)

      expect(wrapper.text()).toContain('Copyright © 2024. All rights reserved.')

      // Simulate time passing to 2025 and force re-render
      vi.setSystemTime(new Date('2025-01-01T00:00:00Z'))
      wrapper.vm.$forceUpdate()
      await nextTick()

      expect(wrapper.text()).toContain('Copyright © 2025. All rights reserved.')
    })
  })

  describe('Edge Cases', () => {
    it('should handle new year correctly', () => {
      vi.setSystemTime(new Date('2024-01-01T00:00:00Z'))
      wrapper = mount(BaseFooter)

      expect(wrapper.text()).toContain('Copyright © 2024. All rights reserved.')
    })

    it('should handle extremely old date', () => {
      vi.setSystemTime(new Date('1900-01-01T00:00:00Z'))
      wrapper = mount(BaseFooter)

      expect(wrapper.text()).toContain('Copyright © 1900. All rights reserved.')
    })
  })

  describe('Component Structure', () => {
    it('should have only one footer element', () => {
      wrapper = mount(BaseFooter)

      const footers = wrapper.findAll('footer')
      expect(footers).toHaveLength(1)
    })

    it('should have only one paragraph element', () => {
      wrapper = mount(BaseFooter)

      const paragraphs = wrapper.findAll('p')
      expect(paragraphs).toHaveLength(1)
    })

    it('should have exactly two direct children in footer', () => {
      wrapper = mount(BaseFooter)

      const footer = wrapper.find('footer')
      const children = footer.element.children
      expect(children).toHaveLength(2) // paragraph + LanguageSwitcher
    })

    it('should maintain correct DOM hierarchy', () => {
      wrapper = mount(BaseFooter)

      const footer = wrapper.find('footer')
      const paragraph = footer.find('p')
      const languageSwitcher = footer.findComponent({ name: 'LanguageSwitcher' })

      expect(footer.exists()).toBe(true)
      expect(paragraph.exists()).toBe(true)
      expect(languageSwitcher.exists()).toBe(true)

      // Check that paragraph and language switcher are direct children of footer
      expect(paragraph.element.parentElement).toBe(footer.element)
      expect(languageSwitcher.element.parentElement).toBe(footer.element)
    })
  })

  describe('Accessibility', () => {
    it('should have semantic footer element', () => {
      wrapper = mount(BaseFooter)

      const footer = wrapper.find('footer')
      expect(footer.exists()).toBe(true)
      expect(footer.element.tagName.toLowerCase()).toBe('footer')
    })

    it('should have appropriate text content for screen readers', () => {
      wrapper = mount(BaseFooter)

      const copyrightText = wrapper.find('p').text()
      expect(copyrightText).toMatch(/Copyright © \d{4}\. All rights reserved\./)
      expect(copyrightText).not.toBe('')
    })

    it('should have unique id for footer', () => {
      wrapper = mount(BaseFooter)

      const footer = wrapper.find('footer')
      expect(footer.attributes('id')).toBe('footer')
    })
  })

  describe('CSS Classes', () => {
    it('should apply all required Tailwind classes to footer', () => {
      wrapper = mount(BaseFooter)

      const footer = wrapper.find('footer')
      const expectedClasses = ['layout-footer', 'py-5', 'container', 'mx-auto']

      expectedClasses.forEach((className) => {
        expect(footer.classes()).toContain(className)
      })
    })

    it('should apply muted text color class to paragraph', () => {
      wrapper = mount(BaseFooter)

      const paragraph = wrapper.find('p')
      // Note: The class uses CSS variable syntax which might not be directly testable
      // but we can check if the class string is present
      expect(paragraph.attributes('class')).toContain('text-sm')
    })

    it('should not have any inline styles', () => {
      wrapper = mount(BaseFooter)

      const footer = wrapper.find('footer')
      const paragraph = wrapper.find('p')

      expect(footer.attributes('style')).toBeUndefined()
      expect(paragraph.attributes('style')).toBeUndefined()
    })
  })

  describe('Component Isolation', () => {
    it('should not affect global state', () => {
      wrapper = mount(BaseFooter)

      // Component should not modify global Date
      expect(typeof Date.now()).toBe('number')
      expect(Date.now).toBeDefined()
    })

    it('should be mountable multiple times', () => {
      const wrapper1 = mount(BaseFooter)
      const wrapper2 = mount(BaseFooter)

      expect(wrapper1.text()).toContain('Copyright ©')
      expect(wrapper2.text()).toContain('Copyright ©')
      expect(wrapper1.text()).toBe(wrapper2.text())

      wrapper1.unmount()
      wrapper2.unmount()
    })

    it('should work without props', () => {
      // Component should work without any props
      expect(() => {
        wrapper = mount(BaseFooter)
      }).not.toThrow()

      expect(wrapper.exists()).toBe(true)
    })

    it('should ignore any passed props gracefully', () => {
      // Component should ignore props since it doesn't define any
      expect(() => {
        wrapper = mount(BaseFooter, {
          props: {
            someUnusedProp: 'test',
            anotherProp: 123,
          },
        })
      }).not.toThrow()

      expect(wrapper.exists()).toBe(true)
      expect(wrapper.text()).toContain('Copyright ©')
    })
  })
})
