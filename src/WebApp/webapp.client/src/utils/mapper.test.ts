import { describe, it, expect } from 'vitest'
import { mapToCreateOrUpdateProductRequest, mapToCreateProductFormData } from './mapper'
import type { CreateOrUpdateProductRequest, Product, ProductFormData } from '@/types/catalog'

describe('mapper', () => {
  describe('mapToCreateProductRequest', () => {
    it('should map complete ProductFormData to CreateProductRequest', () => {
      // Arrange
      const formData: ProductFormData = {
        name: 'Test Product',
        description: 'Test Description',
        price: 99.99,
        productType: { id: 'type-123', name: 'Electronics' },
        brand: { id: 'brand-456', name: 'TestBrand' },
        availableStock: 100,
        restockThreshold: 10,
        maxStockThreshold: 500,
      }

      // Act
      const result = mapToCreateOrUpdateProductRequest(formData)

      // Assert
      expect(result).toEqual({
        name: 'Test Product',
        description: 'Test Description',
        price: 99.99,
        productTypeId: 'type-123',
        brandId: 'brand-456',
        availableStock: 100,
        restockThreshold: 10,
        maxStockThreshold: 500,
      } as CreateOrUpdateProductRequest)
    })

    it('should handle null description by using empty string', () => {
      // Arrange
      const formData: ProductFormData = {
        name: 'Test Product',
        description: null,
        price: 99.99,
        productType: { id: 'type-123', name: 'Electronics' },
        brand: { id: 'brand-456', name: 'TestBrand' },
        availableStock: 100,
        restockThreshold: 10,
        maxStockThreshold: 500,
      }

      // Act
      const result = mapToCreateOrUpdateProductRequest(formData)

      // Assert
      expect(result.description).toBe('')
    })

    it('should handle null productType by using empty string for productTypeId', () => {
      // Arrange
      const formData: ProductFormData = {
        name: 'Test Product',
        description: 'Test Description',
        price: 99.99,
        productType: null,
        brand: { id: 'brand-456', name: 'TestBrand' },
        availableStock: 100,
        restockThreshold: 10,
        maxStockThreshold: 500,
      }

      // Act
      const result = mapToCreateOrUpdateProductRequest(formData)

      // Assert
      expect(result.productTypeId).toBe('')
    })

    it('should handle null brand by using empty string for brandId', () => {
      // Arrange
      const formData: ProductFormData = {
        name: 'Test Product',
        description: 'Test Description',
        price: 99.99,
        productType: { id: 'type-123', name: 'Electronics' },
        brand: null,
        availableStock: 100,
        restockThreshold: 10,
        maxStockThreshold: 500,
      }

      // Act
      const result = mapToCreateOrUpdateProductRequest(formData)

      // Assert
      expect(result.brandId).toBe('')
    })

    it('should handle zero values correctly', () => {
      // Arrange
      const formData: ProductFormData = {
        name: 'Zero Price Product',
        description: 'Free product',
        price: 0,
        productType: { id: 'type-123', name: 'Electronics' },
        brand: { id: 'brand-456', name: 'TestBrand' },
        availableStock: 0,
        restockThreshold: 0,
        maxStockThreshold: 0,
      }

      // Act
      const result = mapToCreateOrUpdateProductRequest(formData)

      // Assert
      expect(result.price).toBe(0)
      expect(result.availableStock).toBe(0)
      expect(result.restockThreshold).toBe(0)
      expect(result.maxStockThreshold).toBe(0)
    })

    it('should handle negative values correctly', () => {
      // Arrange
      const formData: ProductFormData = {
        name: 'Negative Values Product',
        description: 'Product with negative values',
        price: -10.5,
        productType: { id: 'type-123', name: 'Electronics' },
        brand: { id: 'brand-456', name: 'TestBrand' },
        availableStock: -5,
        restockThreshold: -2,
        maxStockThreshold: -100,
      }

      // Act
      const result = mapToCreateOrUpdateProductRequest(formData)

      // Assert
      expect(result.price).toBe(-10.5)
      expect(result.availableStock).toBe(-5)
      expect(result.restockThreshold).toBe(-2)
      expect(result.maxStockThreshold).toBe(-100)
    })

    it('should handle very large numbers correctly', () => {
      // Arrange
      const formData: ProductFormData = {
        name: 'Expensive Product',
        description: 'Very expensive product',
        price: Number.MAX_SAFE_INTEGER,
        productType: { id: 'type-123', name: 'Electronics' },
        brand: { id: 'brand-456', name: 'TestBrand' },
        availableStock: Number.MAX_SAFE_INTEGER,
        restockThreshold: Number.MAX_SAFE_INTEGER,
        maxStockThreshold: Number.MAX_SAFE_INTEGER,
      }

      // Act
      const result = mapToCreateOrUpdateProductRequest(formData)

      // Assert
      expect(result.price).toBe(Number.MAX_SAFE_INTEGER)
      expect(result.availableStock).toBe(Number.MAX_SAFE_INTEGER)
      expect(result.restockThreshold).toBe(Number.MAX_SAFE_INTEGER)
      expect(result.maxStockThreshold).toBe(Number.MAX_SAFE_INTEGER)
    })

    it('should handle empty strings for nested object properties', () => {
      // Arrange
      const formData: ProductFormData = {
        name: 'Test Product',
        description: 'Test Description',
        price: 99.99,
        productType: { id: '', name: 'Electronics' },
        brand: { id: '', name: 'TestBrand' },
        availableStock: 100,
        restockThreshold: 10,
        maxStockThreshold: 500,
      }

      // Act
      const result = mapToCreateOrUpdateProductRequest(formData)

      // Assert
      expect(result.productTypeId).toBe('')
      expect(result.brandId).toBe('')
    })
  })

  describe('mapToCreateProductFormData', () => {
    it('should map complete Product to ProductFormData', () => {
      // Arrange
      const product: Product = {
        id: 'product-789',
        name: 'Test Product',
        description: 'Test Description',
        price: 99.99,
        productTypeId: 'type-123',
        brandId: 'brand-456',
        availableStock: 100,
        restockThreshold: 10,
        maxStockThreshold: 500,
      }

      // Act
      const result = mapToCreateProductFormData(product)

      // Assert
      expect(result).toEqual({
        name: 'Test Product',
        description: 'Test Description',
        price: 99.99,
        brand: { id: 'brand-456', name: '' },
        productType: { id: 'type-123', name: '' },
        availableStock: 100,
        restockThreshold: 10,
        maxStockThreshold: 500,
      } as ProductFormData)
    })

    it('should handle empty string values correctly', () => {
      // Arrange
      const product: Product = {
        id: '',
        name: '',
        description: '',
        price: 0,
        productTypeId: '',
        brandId: '',
        availableStock: 0,
        restockThreshold: 0,
        maxStockThreshold: 0,
      }

      // Act
      const result = mapToCreateProductFormData(product)

      // Assert
      expect(result.name).toBe('')
      expect(result.description).toBe('')
      expect(result.price).toBe(0)
      expect(result.brand?.id).toBe('')
      expect(result.brand?.name).toBe('')
      expect(result.productType?.id).toBe('')
      expect(result.productType?.name).toBe('')
      expect(result.availableStock).toBe(0)
      expect(result.restockThreshold).toBe(0)
      expect(result.maxStockThreshold).toBe(0)
    })

    it('should handle zero values correctly', () => {
      // Arrange
      const product: Product = {
        id: 'product-789',
        name: 'Zero Price Product',
        description: 'Free product',
        price: 0,
        productTypeId: 'type-123',
        brandId: 'brand-456',
        availableStock: 0,
        restockThreshold: 0,
        maxStockThreshold: 0,
      }

      // Act
      const result = mapToCreateProductFormData(product)

      // Assert
      expect(result.price).toBe(0)
      expect(result.availableStock).toBe(0)
      expect(result.restockThreshold).toBe(0)
      expect(result.maxStockThreshold).toBe(0)
    })

    it('should handle negative values correctly', () => {
      // Arrange
      const product: Product = {
        id: 'product-789',
        name: 'Negative Values Product',
        description: 'Product with negative values',
        price: -10.5,
        productTypeId: 'type-123',
        brandId: 'brand-456',
        availableStock: -5,
        restockThreshold: -2,
        maxStockThreshold: -100,
      }

      // Act
      const result = mapToCreateProductFormData(product)

      // Assert
      expect(result.price).toBe(-10.5)
      expect(result.availableStock).toBe(-5)
      expect(result.restockThreshold).toBe(-2)
      expect(result.maxStockThreshold).toBe(-100)
    })

    it('should handle very large numbers correctly', () => {
      // Arrange
      const product: Product = {
        id: 'product-789',
        name: 'Expensive Product',
        description: 'Very expensive product',
        price: Number.MAX_SAFE_INTEGER,
        productTypeId: 'type-123',
        brandId: 'brand-456',
        availableStock: Number.MAX_SAFE_INTEGER,
        restockThreshold: Number.MAX_SAFE_INTEGER,
        maxStockThreshold: Number.MAX_SAFE_INTEGER,
      }

      // Act
      const result = mapToCreateProductFormData(product)

      // Assert
      expect(result.price).toBe(Number.MAX_SAFE_INTEGER)
      expect(result.availableStock).toBe(Number.MAX_SAFE_INTEGER)
      expect(result.restockThreshold).toBe(Number.MAX_SAFE_INTEGER)
      expect(result.maxStockThreshold).toBe(Number.MAX_SAFE_INTEGER)
    })

    it('should always set brand and productType name to empty string', () => {
      // Arrange
      const product: Product = {
        id: 'product-789',
        name: 'Test Product',
        description: 'Test Description',
        price: 99.99,
        productTypeId: 'type-123',
        brandId: 'brand-456',
        availableStock: 100,
        restockThreshold: 10,
        maxStockThreshold: 500,
      }

      // Act
      const result = mapToCreateProductFormData(product)

      // Assert
      expect(result.brand?.name).toBe('')
      expect(result.productType?.name).toBe('')
    })

    it('should handle special characters in strings', () => {
      // Arrange
      const product: Product = {
        id: 'product-789',
        name: 'Test Product with Special Characters: !@#$%^&*()',
        description: 'Description with unicode: 你好 世界 🌍',
        price: 99.99,
        productTypeId: 'type-with-special-chars!@#',
        brandId: 'brand-with-unicode-你好',
        availableStock: 100,
        restockThreshold: 10,
        maxStockThreshold: 500,
      }

      // Act
      const result = mapToCreateProductFormData(product)

      // Assert
      expect(result.name).toBe('Test Product with Special Characters: !@#$%^&*()')
      expect(result.description).toBe('Description with unicode: 你好 世界 🌍')
      expect(result.brand?.id).toBe('brand-with-unicode-你好')
      expect(result.productType?.id).toBe('type-with-special-chars!@#')
    })
  })

  describe('round-trip mapping', () => {
    it('should preserve data integrity when mapping Product -> FormData -> CreateRequest', () => {
      // Arrange
      const originalProduct: Product = {
        id: 'product-789',
        name: 'Round Trip Product',
        description: 'Test round trip mapping',
        price: 149.99,
        productTypeId: 'type-roundtrip',
        brandId: 'brand-roundtrip',
        availableStock: 50,
        restockThreshold: 5,
        maxStockThreshold: 200,
      }

      // Act
      const formData = mapToCreateProductFormData(originalProduct)
      const createRequest = mapToCreateOrUpdateProductRequest(formData)

      // Assert
      expect(createRequest.name).toBe(originalProduct.name)
      expect(createRequest.description).toBe(originalProduct.description)
      expect(createRequest.price).toBe(originalProduct.price)
      expect(createRequest.productTypeId).toBe(originalProduct.productTypeId)
      expect(createRequest.brandId).toBe(originalProduct.brandId)
      expect(createRequest.availableStock).toBe(originalProduct.availableStock)
      expect(createRequest.restockThreshold).toBe(originalProduct.restockThreshold)
      expect(createRequest.maxStockThreshold).toBe(originalProduct.maxStockThreshold)
    })
  })
})
