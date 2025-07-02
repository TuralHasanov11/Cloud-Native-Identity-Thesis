import type { MenuItem } from 'primevue/menuitem'

export type BaseMenuItemProp = MenuItem & {
  roles?: string[]
  groups?: string[]
}
