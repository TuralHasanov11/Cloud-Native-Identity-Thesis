import { HttpStatusCode } from '@/types/common'
import { createFetch } from '@vueuse/core'
import { useToast } from 'primevue'

const baseUrl = import.meta.env.VITE_API_URI as string

const useBffFetch = createFetch({
  baseUrl: baseUrl,
  fetchOptions: {
    mode: 'cors',
    credentials: 'include',
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
  },
  options: {
    onFetchError(ctx) {
      const toast = useToast()

      if (ctx.response?.status === HttpStatusCode.Unauthorized || ctx.response?.status === HttpStatusCode.Forbidden) {
        toast.add({
          severity: 'error',
          summary: 'Authentication Error',
          detail: 'You are not authorized to perform this action.',
          life: 5000,
        })
      }

      console.log(ctx.error)

      return ctx
    },
  },
})

export default useBffFetch
