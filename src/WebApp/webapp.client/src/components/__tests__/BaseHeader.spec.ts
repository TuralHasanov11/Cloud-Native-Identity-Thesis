import { describe, it, expect, vi } from 'vitest'
import { mount } from '@vue/test-utils'
import BaseHeader from '../BaseHeader.vue'
import { nextTick } from 'vue'

describe('BaseHeader', () => {
  it('renders properly', () => {
    const wrapper = mount(BaseHeader)
    expect(wrapper.text()).toContain('Home')
    expect(wrapper.text()).toContain('Products')
    expect(wrapper.text()).toContain('Categories')
  })

  it('renders user menu when user is logged in', async () => {
    const mockUser = { name: 'John Doe' }
    const wrapper = mount(BaseHeader, {
      global: {
        mocks: {
          useIdentity: () => ({
            user: { value: mockUser },
            login: vi.fn(),
            logout: vi.fn(),
          }),
        },
      },
    })

    await nextTick()
    expect(wrapper.text()).toContain(mockUser.name)
    expect(wrapper.text()).toContain('Orders')
    expect(wrapper.text()).toContain('Profile')
    expect(wrapper.text()).toContain('Logout')
  })

  it('renders login button when user is not logged in', async () => {
    const wrapper = mount(BaseHeader, {
      global: {
        mocks: {
          useIdentity: () => ({
            user: { value: null },
            login: vi.fn(),
            logout: vi.fn(),
          }),
        },
      },
    })

    await nextTick()
    expect(wrapper.text()).toContain('Sign in')
  })

  it('calls login function when login button is clicked', async () => {
    const loginMock = vi.fn()
    const wrapper = mount(BaseHeader, {
      global: {
        mocks: {
          useIdentity: () => ({
            user: { value: null },
            login: loginMock,
            logout: vi.fn(),
          }),
        },
      },
    })

    await wrapper.find('[label="Sign in"]').trigger('click')
    expect(loginMock).toHaveBeenCalled()
  })

  it('calls logout function when logout button is clicked', async () => {
    const logoutMock = vi.fn()
    const mockUser = { name: 'John Doe' }
    const wrapper = mount(BaseHeader, {
      global: {
        mocks: {
          useIdentity: () => ({
            user: { value: mockUser },
            login: vi.fn(),
            logout: logoutMock,
          }),
        },
      },
    })

    await wrapper.find('[label="Logout"]').trigger('click')
    expect(logoutMock).toHaveBeenCalled()
  })

  it('highlights the active menu item based on the route', async () => {
    const mockRoute = { path: '/products' }
    const wrapper = mount(BaseHeader, {
      global: {
        mocks: {
          useRoute: () => mockRoute,
        },
      },
    })

    await nextTick()
    const activeItem = wrapper.find('.active')
    expect(activeItem.text()).toContain('Products')
  })
})
