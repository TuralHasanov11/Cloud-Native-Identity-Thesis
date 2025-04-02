
export function useHelpers() {
  const route = useRoute();
  const runtimeConfig = useRuntimeConfig();

  const isShowingMobileMenu = useState<boolean>('isShowingMobileMenu', () => false);
  const productsPerPage: number = runtimeConfig.public?.PRODUCTS_PER_PAGE as number || 24;
  const isDev: boolean = process.env.NODE_ENV === 'development';
  const FALLBACK_IMG = '/images/placeholder.jpg';

  function toggleMobileMenu(state: boolean | undefined = undefined): void {
    isShowingMobileMenu.value = state ?? !isShowingMobileMenu.value;
  }

  function clearAllLocalStorage(): void {
    localStorage.clear();
  }

  function replaceQueryParam(param: string, newval: string, search: string): string {
    const regex = new RegExp('([?;&])' + param + '[^&;]*[;&]?');
    const query = search.replace(regex, '$1').replace(/&$/, '');
    return (query.length > 2 ? query + '&' : '?') + (newval ? param + '=' + newval : '');
  }

  function removeBodyClass(className: string): void {
    const body = document.querySelector('body');
    body?.classList.remove(className);
  }

  function addBodyClass(className: string): void {
    const body = document.querySelector('body');
    body?.classList.add(className);
  }

  function toggleBodyClass(className: string): void {
    const body = document.querySelector('body');
    if (body?.classList.contains(className)) {
      body.classList.remove(className);
    } else {
      body?.classList.add(className);
    }
  }

  const isQueryEmpty = computed<boolean>(() => Object.keys(route.query).length === 0);

  const formatDate = (date?: string | null): string => {
    if (!date) return '';
    return new Date(date).toLocaleDateString('en-US', { month: 'long', day: 'numeric', year: 'numeric' });
  };

  const formatPrice = (price: number): string => price.toLocaleString('en-US', { style: 'currency', currency: 'EUR' });

  const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  return {
    isShowingMobileMenu,
    productsPerPage,
    isQueryEmpty,
    isDev,
    FALLBACK_IMG,
    clearAllLocalStorage,
    replaceQueryParam,
    addBodyClass,
    removeBodyClass,
    toggleBodyClass,
    toggleMobileMenu,
    formatDate,
    formatPrice,
    scrollToTop
  };
}
