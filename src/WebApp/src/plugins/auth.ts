import useIdentity from '@/composables/useIdentity'

export default {
  async install() {
    const { getUserInfo } = useIdentity()
    await getUserInfo()
  },
}
