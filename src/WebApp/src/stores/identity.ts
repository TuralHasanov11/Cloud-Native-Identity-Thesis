// import useAppFetch from '@/composables/useAppFetch'
import useBffFetch from '@/composables/useBffFetch'
import type { User } from '@/types/identity'
import { defineStore } from 'pinia'
import { ref, type Ref } from 'vue'

export const GUEST_USER: User = {
  id: '',
  name: 'Guest',
  email: '',
  address: {
    city: '',
    country: '',
    state: '',
    street: '',
    zipCode: '',
  },
  //   roles: [],
}

interface IdentityStore {
  user: Ref<User>
  getUserInfo: () => Promise<void>
}

export const useIdentityStore = defineStore('identity', (): IdentityStore => {
  const user = ref<User>(GUEST_USER)

  async function getUserInfo(): Promise<void> {
    try {
      const { data } = await useBffFetch<User>('/api/identity/user-info').get().json<User>()
      if (data.value != null) {
        user.value = data.value
      }
    } catch (error) {
      console.error(error)
      user.value = GUEST_USER
    }
  }

  return { user, getUserInfo }
})
