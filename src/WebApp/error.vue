<script setup lang="ts">
import type { NuxtError } from "#app";
import * as locales from "@nuxt/ui/locale";

const { locale } = useI18n();

defineProps<{
  error: NuxtError;
}>();

const lang = computed(() => locales[locale.value].code);
const dir = computed(() => locales[locale.value].dir);

useHead({
  htmlAttrs: {
    lang,
    dir,
  },
});

useSeoMeta({
  title: "Page not found",
  description: "We are sorry but this page could not be found.",
});
</script>

<template>
  <div>
    <BaseHeader />

    <UMain>
      <UContainer>
        <UPage>
          <UError :error="error" />
        </UPage>
      </UContainer>
    </UMain>

    <BaseFooter />

    <UToaster />
  </div>
</template>
