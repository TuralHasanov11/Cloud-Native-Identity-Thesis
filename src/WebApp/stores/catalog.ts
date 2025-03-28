import { defineStore } from "pinia";
import type { ShallowRef } from "vue";
import type { Category, Product } from "~/types/catalog";

export interface CatalogStore {
  popularProducts: ShallowRef<Product[]>;
  categories: ShallowRef<Category[]>;
  categoryProducts: ShallowRef<Product[]>;
  products: ShallowRef<Product[]>;
  getProducts: (payload: GetProductsRequest) => Promise<void>;
}

export interface GetProductsRequest{
    name?: string
    productType?: string
    category?: string
    pageSize?: number
    pageCursor?: string
}

export const useCatalogStore = defineStore("catalog", (): CatalogStore => {
  const popularProducts = shallowRef<Product[]>([]);
  const categories = shallowRef<Category[]>([]);
  const categoryProducts = ref<Product[]>([]);
  const products = ref<Product[]>([]);

  async function getProducts(payload: GetProductsRequest) : Promise<void> {
    
  }

  return {
    popularProducts,
    categories,
    categoryProducts,
    products,
    getProducts
  };
});
