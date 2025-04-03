<script lang="ts" setup>
import type { BasketItem } from '~/types/basket';
import { StockStatusEnum, type Product } from '~/types/catalog';

const route = useRoute();
const { storeSettings } = useAppConfig();
const { addToCart, isUpdatingCart } = useBasket();
const {product, getProductById} = useCatalog()
const { t } = useI18n();
const slug = route.params.slug as string;
const selectProductInput = computed<BasketItem>(() => ({
  productId: product.value?.id ?? '',
  quantity: quantity.value
}));

await getProductById(slug);

if (!product.value) {
  throw showError({ statusCode: 404, statusMessage: t('messages.shop.productNotFound') });
}

const quantity = ref<number>(1);

const mergeLiveStockStatus = (payload: Product): void => {
    console.log('mergeLiveStockStatus', payload);
//   product.value.stockStatus = payload.stockStatus ?? product.value?.stockStatus;

};

const stockStatus = computed<StockStatusEnum>(() => {
  return product.value != null && product.value.availableStock > 0 ? StockStatusEnum.IN_STOCK : StockStatusEnum.OUT_OF_STOCK;
});
const disabledAddToCart = computed(() => {
  return product.value != null || stockStatus.value === StockStatusEnum.OUT_OF_STOCK ||  isUpdatingCart.value;
});
</script>

<template>
  <main id="product" class="container relative py-6 xl:max-w-7xl">

    <UContainer>
      <!-- <UPageHeader
      v-bind="page"
      class="py-[50px]"
    /> -->

      <UPageBody>
        <div v-if="product">
      <!-- <Breadcrumb v-if="storeSettings.showBreadcrumbOnSingleProduct" :product class="mb-6" /> -->

      <div class="flex flex-col gap-10 md:flex-row md:justify-between lg:gap-24">
        <!-- <ProductImageGallery
          v-if="product.image"
          class="relative flex-1"
          :main-image="product.image"
          :gallery="product.galleryImages!"
          :node="type"
          :active-variation="activeVariation || {}" />
        <NuxtImg v-else class="relative flex-1 skeleton" src="/images/placeholder.jpg" :alt="product?.name || 'Product'" /> -->

        <div class="lg:max-w-md xl:max-w-lg md:py-2 w-full">
          <div class="flex justify-between mb-4">
            <div class="flex-1">
              <h1 class="flex flex-wrap items-center gap-2 mb-2 text-2xl font-sesmibold">
                {{ product.name }}
              </h1>
            </div>
            <ProductPrice class="text-xl" :sale-price="product.price" :regular-price="product.price" />
          </div>

          <div class="grid gap-2 my-8 text-sm empty:hidden">
            <div class="flex items-center gap-2">
              <span class="text-gray-400">{{ $t('messages.shop.availability') }}: </span>
              <StockStatus :stock-status="stockStatus" @updated="mergeLiveStockStatus" />
            </div>
            <!-- <div v-if="storeSettings.showSKU && product.sku" class="flex items-center gap-2">
              <span class="text-gray-400">{{ $t('messages.shop.sku') }}: </span>
              <span>{{ product.sku || 'N/A' }}</span>
            </div> -->
          </div>

          <div class="mb-8 font-light prose">{{product.description}}</div>

          <hr >

          <!-- <form @submit.prevent="addToCart(selectProductInput)">
            <div
              class="fixed bottom-0 left-0 z-10 flex items-center w-full gap-4 p-4 mt-12 bg-white md:static md:bg-transparent bg-opacity-90 md:p-0">
              <input
                v-model="quantity"
                type="number"
                min="1"
                aria-label="Quantity"
                class="bg-white border rounded-lg flex text-left p-2.5 w-20 gap-4 items-center justify-center focus:outline-none" >
              <AddToCartButton class="flex-1 w-full md:max-w-xs" :disabled="disabledAddToCart" :class="{ loading: isUpdatingCart }" />
            </div>
          </form> -->

          <div v-if="storeSettings.showProductCategoriesOnSingleProduct && product.categories">
            <div class="grid gap-2 my-8 text-sm">
              <div class="flex items-center gap-2">
                <span class="text-gray-400">{{ $t('messages.shop.category', 2) }}:</span>
                <div class="product-categories">
                  <NuxtLink
                    v-for="category in product.categories"
                    :key="category.id"
                    :to="`/product-category/${decodeURIComponent(category?.slug || '')}`"
                    class="hover:text-primary"
                    :title="category.name"
                    >{{ category.name }}<span class="comma">, </span>
                  </NuxtLink>
                </div>
              </div>
            </div>
            <hr >
          </div>

          <div class="flex flex-wrap gap-4">
            <!-- <WishlistButton :product />
            <ShareButton :product /> -->
          </div>
        </div>
      </div>
      <div v-if="product.description" class="my-32">
        <ProductTabs :product />
      </div>
      <!-- <div v-if="product.related && storeSettings.showRelatedProducts" class="my-32">
        <div class="mb-4 text-xl font-semibold">{{ $t('messages.shop.youMayLike') }}</div>
        <ProductRow :products="product.related.nodes" class="grid-cols-2 md:grid-cols-4 lg:grid-cols-5" />
      </div> -->
    </div>
      </UPageBody>
    </UContainer>

    
  </main>
</template>

<style scoped>
.product-categories > a:last-child .comma {
  display: none;
}

input[type='number']::-webkit-inner-spin-button {
  opacity: 1;
}
</style>
