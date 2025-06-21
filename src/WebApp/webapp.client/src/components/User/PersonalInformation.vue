<script setup lang="ts">
import useIdentity from '@/composables/useIdentity'
import { useI18n } from 'vue-i18n'
const { t } = useI18n()
const { user } = useIdentity()
</script>

<template>
  <form v-if="user" class="bg-white rounded-lg shadow">
    <div class="grid gap-6 p-8 md:grid-cols-2">
      <h3 class="text-xl font-semibold col-span-full">
        {{ t('messages.account.personalInfo') }}
      </h3>

      {{ user?.name }}

      {{ user?.email }}
    </div>
  </form>
</template>
