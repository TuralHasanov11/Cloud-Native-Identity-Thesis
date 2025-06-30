import type { CreateOrUpdateProductRequest, Product, ProductFormData } from '@/types/catalog'

export function mapToCreateOrUpdateProductRequest(formData: ProductFormData): CreateOrUpdateProductRequest {
  return {
    name: formData.name,
    description: formData.description ?? '',
    price: formData.price,
    productTypeId: formData.productType?.id ?? '',
    brandId: formData.brand?.id ?? '',
    availableStock: formData.availableStock,
    restockThreshold: formData.restockThreshold,
    maxStockThreshold: formData.maxStockThreshold,
  }
}

export function mapToCreateProductFormData(value: Product): ProductFormData {
  return {
    name: value.name,
    description: value.description,
    price: value.price,
    brand: { id: value.brandId, name: '' },
    productType: { id: value.productTypeId, name: '' },
    availableStock: value.availableStock,
    restockThreshold: value.restockThreshold,
    maxStockThreshold: value.maxStockThreshold,
  }
}
