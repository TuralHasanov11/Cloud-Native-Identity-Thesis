import { ref } from 'vue'

const isShowingMobileMenu = ref<boolean>(false)

export function useHelpers() {
  const FALLBACK_IMG = '/images/placeholder.jpg'

  const formatDate = (date?: string | null): string => {
    if (!date) return ''
    return new Date(date).toLocaleDateString('en-US', {
      month: 'long',
      day: 'numeric',
      year: 'numeric',
    })
  }

  function toggleMobileMenu(state: boolean | undefined = undefined): void {
    isShowingMobileMenu.value = state ?? !isShowingMobileMenu.value
  }

  const formatPrice = (price: number): string =>
    price.toLocaleString('en-US', { style: 'currency', currency: 'EUR' })

  const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' })
  }

  function removeBodyClass(className: string): void {
    const body = document.querySelector('body')
    body?.classList.remove(className)
  }

  function addBodyClass(className: string): void {
    const body = document.querySelector('body')
    body?.classList.add(className)
  }

  function toggleBodyClass(className: string): void {
    const body = document.querySelector('body')
    if (body?.classList.contains(className)) {
      body.classList.remove(className)
    } else {
      body?.classList.add(className)
    }
  }

  return {
    FALLBACK_IMG,
    formatDate,
    formatPrice,
    scrollToTop,
    removeBodyClass,
    addBodyClass,
    toggleBodyClass,
    isShowingMobileMenu,
    toggleMobileMenu,
  }
}
