import useIdentity from '@/composables/useIdentity'

export default {
  async install() {
    try {
      const { getUserInfo } = useIdentity()
      await getUserInfo()
    } catch (error: unknown) {
      console.error('Error during authentication:', error)
    }
  },
}
