export type CardType = {
  id: number
  name: string
}

export const CARD_TYPE_NULL_OBJECT: CardType = {
  id: 0,
  name: '',
}

export type OrderSummary = {
  orderNumber: string
  date: string
  status: string
  total: number
}

export type ShipOrderRequest = {
  orderNumber: string
}

export type OrderItem = {
  productId: string
  productName: string
  pictureUrl: string
  unitPrice: number
  units: number
  discount: number
  totalPrice: number
}

export type Order = {
  orderNumber: string
  date: string
  description: string
  city: string
  country: string
  state: string
  street: string
  zipcode: string
  status: OrderStatus
  total: number
  orderItems: OrderItem[]
}

export enum OrderStatus {
  Default = 'Default',
  Submitted = 'Submitted',
  AwaitingValidation = 'AwaitingValidation',
  StockConfirmed = 'StockConfirmed',
  Paid = 'Paid',
  Shipped = 'Shipped',
  Cancelled = 'Cancelled',
}

export type OrderDraft = {
  orderItems: OrderItem[]
  total: number
}

export type DraftOrderRequest = {
  customerId: string
  items: OrderItem[]
}

export type CreateOrderRequest = {
  userId: string
  userName: string
  city: string
  street: string
  state: string
  country: string
  zipCode: string
  cardTypeId: number
  customer: string
  items: BasketItem[]
}

export type CancelOrderRequest = {
  orderNumber: string
}

export type BasketItem = {
  productId: string
  productName: string
  unitPrice: number
  oldUnitPrice?: number
  quantity: number
  pictureUrl: string
}

export interface Customer {
  id: string
  name: string
  identityId: string
  paymentMethods: PaymentMethod[]
}

export type PaymentMethod = {
  alias: string
  cardTypeId: number
  cardType: CardType
}

export const GUEST_CUSTOMER: Customer = {
  id: '',
  name: '',
  identityId: '',
  paymentMethods: [],
}

export const CUSTOMER_ORDER_NULL_OBJECT: Order = {
  orderNumber: '',
  date: '',
  description: '',
  city: '',
  country: '',
  state: '',
  street: '',
  zipcode: '',
  status: OrderStatus.Default,
  total: 0,
  orderItems: [],
}
