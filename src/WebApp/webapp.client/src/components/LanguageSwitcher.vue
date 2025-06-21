<script setup lang="ts">
import type { SelectChangeEvent } from 'primevue'
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'

const { locale } = useI18n()
const currentLocale = ref(locale.value)

const locales = ref([
  { code: 'en', name: 'English' },
  { code: 'de', name: 'Deutsch' },
])

function onChangeLanguage(payload: SelectChangeEvent) {
  locale.value = payload.value.code as string
}
</script>

<template>
  <Select
    :options="locales"
    optionLabel="name"
    variant="filled"
    v-model="currentLocale"
    class="w-48"
    @change="onChangeLanguage"
  />
</template>
