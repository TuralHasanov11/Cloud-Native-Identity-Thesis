import { HttpStatusCode } from '@/types/common'
import { createFetch } from '@vueuse/core'

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
      if (ctx.response?.status === HttpStatusCode.Unauthorized || ctx.response?.status === HttpStatusCode.Forbidden) {
        ctx.error = {
          status: ctx.response?.status,
          message: 'You are not authorized to access this resource.',
        }
      }

      console.log(ctx.error)

      return ctx
    },
  },
})

export default useBffFetch
