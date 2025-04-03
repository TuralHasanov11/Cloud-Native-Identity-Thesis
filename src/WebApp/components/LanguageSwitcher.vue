<script setup lang="ts">
const { locales, locale, setLocaleCookie } = useI18n();

const languages = computed(() => locales.value.map((l) => {
  return {
    label: l.name,
    name: l.code,
  };
}));

function onChangeLanguage(payload: Event){
  // setLocaleCookie(newLocale);
}

</script>

<template>
  <USelect :items="languages" class="w-48" @change="onChangeLanguage" />
</template>
