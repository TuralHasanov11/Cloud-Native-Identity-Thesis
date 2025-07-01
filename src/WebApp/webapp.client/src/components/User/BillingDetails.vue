<script lang="ts" setup>
import useIdentity from '@/composables/identity/useIdentity'
import type { Address } from '@/types/identity'
import { useI18n } from 'vue-i18n'
const { t } = useI18n()

const { user } = useIdentity()
const address = defineModel<Address>('address', { required: true })
</script>

<template>
  <div class="grid w-full gap-4 lg:grid-cols-3">
    <div class="w-full flex flex-col gap-1">
      <label for="first-name">{{ t('messages.billing.firstName') }}</label>
      <InputText id="first-name" disabled v-model="user.name" placeholder="John" autocomplete="given-name" type="text"
        required />
    </div>

    <div class="w-full col-span-full flex flex-col gap-1">
      <label for="address1">{{ t('messages.billing.address1') }}</label>
      <InputText id="address1" disabled v-model="address.street" placeholder="O'Connell Street 47"
        autocomplete="street-address" type="text" required />
    </div>

    <div class="w-full flex flex-col gap-1">
      <label for="city">{{ t('messages.billing.city') }}</label>
      <InputText id="city" disabled v-model="address.city" placeholder="New York" autocomplete="locality" type="text"
        required />
    </div>

    <div class="w-full flex flex-col gap-1">
      <label for="state">{{ t('messages.billing.state') }} ({{ t('messages.general.optional') }})</label>
      <Select id="state" disabled v-model="address.state" :value="address.state" :country-code="address.country"
        autocomplete="address-level1" />
    </div>

    <div class="w-full flex flex-col gap-1">
      <label for="country">{{ t('messages.billing.country') }}</label>
      <Select id="country" disabled v-model="address.country" :value="address.country" autocomplete="country" />
    </div>

    <div class="w-full flex flex-col gap-1">
      <label for="zip">{{ t('messages.billing.zip') }}</label>
      <InputText id="zip" disabled v-model="address.zipCode" placeholder="10001" autocomplete="postal-code" type="text"
        required />
    </div>
  </div>
</template>
