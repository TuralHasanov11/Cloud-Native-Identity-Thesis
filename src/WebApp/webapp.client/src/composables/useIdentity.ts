import { GUEST_USER, useIdentityStore } from '@/stores/identity'
import type { User } from '@/types/identity'
import { computed } from 'vue'

export default function useIdentity() {
  const identityStore = useIdentityStore()

  const user = computed<User>(() => identityStore.user)
  const isAuthenticated = computed<boolean>(() => user.value && user.value.id !== GUEST_USER.id)
  const isGuest = computed<boolean>(() => !user.value || user.value.id === GUEST_USER.id)

  function hasRole(roles: string[]): boolean {
    return user.value?.roles?.some((r) => roles.includes(r)) ?? false
  }

  function hasGroup(groups: string[]): boolean {
    return user.value?.groups?.some((r) => groups.includes(r)) ?? false
  }

  function login(returnUrl: string = '/'): void {
    window.location.href = `/identity/login?returnUrl=${returnUrl}`
  }

  function logout(): void {
    window.location.href = '/identity/logout'
  }

  return {
    user,
    isAuthenticated,
    isGuest,
    hasRole,
    hasGroup,
    getUserInfo: identityStore.getUserInfo,
    login,
    logout,
  }
}
