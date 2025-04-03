export default function useCustomer(){
    const customerStore = useCustomerStore();

    const isGuest = computed<boolean>(() => customerStore.customer.id === GUEST_CUSTOMER.id);

    return {
        customer: computed(() => customerStore.customer),
        order: computed(() => customerStore.order),
        orders: computed(() => customerStore.orders),
        getOrder: customerStore.getOrder,
        getOrders: customerStore.getOrders,
        isGuest,
    }
}