import useIdentity from '@/composables/identity/useIdentity'
import type { CustomRouteMeta } from '@/types/common'
import type { NavigationGuardNext, RouteLocationNormalizedGeneric, RouteLocationNormalizedLoadedGeneric } from 'vue-router'

const guard = async (to: RouteLocationNormalizedGeneric, from: RouteLocationNormalizedLoadedGeneric, next: NavigationGuardNext) => {
  const { hasRole, isAuthenticated, login } = useIdentity()
  const toRouteMeta = to.meta as CustomRouteMeta

  if (toRouteMeta.requireRole && !hasRole(toRouteMeta.requireRole)) {
    return next({ name: 'home' })
  } else if (toRouteMeta.requireAuth === true && isAuthenticated.value === false) {
    login()
    return next({ name: 'home' })
  } else {
    return next()
  }
}

export default guard
