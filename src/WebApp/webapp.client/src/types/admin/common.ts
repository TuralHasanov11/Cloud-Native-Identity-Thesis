export interface AdminMenuItemProp {
  label: string
  icon?: string
  to?: string
  url?: string
  class?: string
  target?: string
  visible?: boolean
  disabled?: boolean
  items?: AdminMenuItemProp[]
  command?: (options: { originalEvent: MouseEvent; item: AdminMenuItemProp }) => void
  separator?: boolean
}
