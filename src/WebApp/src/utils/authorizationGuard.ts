import useIdentity from '@/composables/useIdentity'
import type { CustomRouteMeta } from '@/types/common'
import type {
  NavigationGuardNext,
  RouteLocationNormalizedGeneric,
  RouteLocationNormalizedLoadedGeneric,
} from 'vue-router'

const guard = async (
  to: RouteLocationNormalizedGeneric,
  from: RouteLocationNormalizedLoadedGeneric,
  next: NavigationGuardNext,
) => {
  const { hasRole, isAuthenticated, login } = useIdentity()
  const toRouteMeta = to.meta as CustomRouteMeta
  console.log(toRouteMeta.requireAuth, isAuthenticated.value)
  console.log(toRouteMeta.requireAuth === true && isAuthenticated.value === false)
  if (toRouteMeta.requireRole && !hasRole(toRouteMeta.requireRole)) {
    return next({ name: 'home' })
  } else if (toRouteMeta.requireAuth === true && isAuthenticated.value === false) {
    return login()
  } else {
    return next()
  }
}

export default guard
