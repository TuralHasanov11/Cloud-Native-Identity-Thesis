<script lang="ts" setup>
import type { CustomerAddress } from '@/types/ordering';

const { user } = useIdentity()
const address = defineModel<CustomerAddress>('address', { required: true })

</script>

<template>
    <div class="grid w-full gap-4 lg:grid-cols-2">
        <div class="w-full">
            <label for="first-name">{{ $t('messages.billing.firstName') }}</label>
            <UInput id="first-name" disabled v-model="user.name" placeholder="John" autocomplete="given-name"
                type="text" required />
        </div>

        <div class="w-full col-span-full">
            <label for="address1">{{ $t('messages.billing.address1') }}</label>
            <UInput id="address1" disabled v-model="address.street" placeholder="O'Connell Street 47"
                autocomplete="street-address" type="text" required />
        </div>

        <div class="w-full">
            <label for="city">{{ $t('messages.billing.city') }}</label>
            <UInput id="city" disabled v-model="address.city" placeholder="New York" autocomplete="locality" type="text"
                required />
        </div>

        <div class="w-full">
            <label for="state">{{ $t('messages.billing.state') }} ({{ $t('messages.general.optional') }})</label>
            <USelect id="state" disabled v-model="address.state" :default-value="address.state"
                :country-code="address.country" autocomplete="address-level1" />
        </div>

        <div class="w-full">
            <label for="country">{{ $t('messages.billing.country') }}</label>
            <USelect id="country" disabled v-model="address.country" :default-value="address.country"
                autocomplete="country" />
        </div>

        <div class="w-full">
            <label for="zip">{{ $t('messages.billing.zip') }}</label>
            <UInput id="zip" disabled v-model="address.zipCode" placeholder="10001" autocomplete="postal-code"
                type="text" required />
        </div>
    </div>
</template>
