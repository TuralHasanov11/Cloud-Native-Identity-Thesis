export default function useCatalog(){
    const catalogStore = useCatalogStore();

    return {
        popularProducts: computed(() => catalogStore.popularProducts),
        categories: computed(() => catalogStore.categories),
        categoryProducts: computed(() => catalogStore.categoryProducts),
        products: computed(() => catalogStore.products),
        getProducts: catalogStore.getProducts
    }
}