import type { Product } from "~/types/catalog";

export default function useCatalog(){
    const catalogStore = useCatalogStore();

    const product = ref<Product | null>(null);

    async function getProductById(id: string) : Promise<void> {
        console.log("getProductById", id);
    }  

    return {
        popularProducts: computed(() => catalogStore.popularProducts),
        brands: computed(() => catalogStore.brands),
        products: computed(() => catalogStore.products),
        productTypes: computed(() => catalogStore.productTypes),
        product,
        getProducts: catalogStore.getProducts,
        getBrands: catalogStore.getBrands,
        getProductTypes: catalogStore.getProductTypes,
        getProductById
    }
}