import { HttpStatusCode } from '@/types/common'
import { createFetch } from '@vueuse/core'
import useIdentity from './useIdentity'

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
      const { login } = useIdentity()

      if (
        ctx.response?.status === HttpStatusCode.Unauthorized ||
        ctx.response?.status === HttpStatusCode.Forbidden
      ) {
        login('/')
      }

      console.log(ctx.error)

      return ctx
    },
  },
})

export default useBffFetch
