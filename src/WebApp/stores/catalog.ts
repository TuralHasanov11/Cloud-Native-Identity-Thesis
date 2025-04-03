import { defineStore } from "pinia";
import type { ShallowRef } from "vue";
import type { Brand, Product, ProductType } from "~/types/catalog";
import type { PaginationResponse } from "~/types/pagination";

export interface CatalogStore {
  popularProducts: ShallowRef<Product[]>;
  brands: ShallowRef<Brand[]>;
  products: ShallowRef<PaginationResponse<Product, string>>;
  productTypes: ShallowRef<ProductType[]>;
  getBrands: () => Promise<void>;
  getProductTypes: () => Promise<void>;
  getProducts: (payload?: GetProductsRequest) => Promise<void>;
}

export interface GetProductsRequest {
  name?: string;
  productType?: string;
  brand?: string;
  pageSize?: number;
  pageCursor?: string;
}

export const PAGINATED_PRODUCTS_NULL_OBJECT: PaginationResponse<
  Product,
  string
> = {
  data: [],
  pageSize: 0,
  pageCursor: "",
  count: 0,
};

export const useCatalogStore = defineStore("catalog", (): CatalogStore => {
  const popularProducts = shallowRef<Product[]>([]);
  const brands = shallowRef<Brand[]>([]);
  const products = shallowRef<PaginationResponse<Product, string>>(
    PAGINATED_PRODUCTS_NULL_OBJECT
  );
  const productTypes = shallowRef<ProductType[]>([]);

  async function getProducts(payload?: GetProductsRequest): Promise<void> {
    console.log("getProducts", payload);
  }

  async function getBrands(): Promise<void> {
    const { data } = await useFetch<Brand[]>("/api/gateway/catalog/brands");

    if (data.value) {
      brands.value = data.value;
    }
  }

  async function getProductTypes(): Promise<void> {
    const { data } = await useFetch<ProductType[]>(
      "/api/gateway/catalog/product-types"
    );

    if (data.value) {
      productTypes.value = data.value;
    }
  }

  return {
    popularProducts,
    brands,
    products,
    productTypes,
    getBrands,
    getProductTypes,
    getProducts,
  };
});
