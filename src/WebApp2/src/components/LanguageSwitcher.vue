<script setup lang="ts">
import { computed } from 'vue';
import { useI18n } from 'vue-i18n'

const { locale } = useI18n();

const locales = [
  { code: 'en', name: 'English' },
  { code: 'de', name: 'Deutsch' },
]

const languages = computed(() => locales.map((l) => {
  return {
    label: l.name,
    name: l.code,
  };
}));

function onChangeLanguage(payload: Event) {
  console.log(payload);
}

</script>

<template>
  <USelect :items="languages" v-model="locale" class="w-48 mb-5" @change="onChangeLanguage" />
</template>
