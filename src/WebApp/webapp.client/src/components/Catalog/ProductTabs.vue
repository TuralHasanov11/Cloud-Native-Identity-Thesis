<script lang="ts" setup>
import type { Product } from '@/types/catalog'
import { computed } from 'vue'

const { product } = defineProps<{
  product: Product
}>()

const tabs = computed(() => [
  {
    label: 'Description',
    icon: 'i-lucide-user',
    content: product?.description,
  },
])
</script>

<template>
  <Tabs value="Description" class="w-full">
    <TabList>
      <Tab v-for="tab in tabs" :key="tab.label" :value="tab.content">{{ tab.label }}</Tab>
    </TabList>
    <TabPanels>
      <TabPanel v-for="tab in tabs" :key="tab.content" :value="tab.content">
        <p class="m-0">{{ tab.content }}</p>
      </TabPanel>
    </TabPanels>
  </Tabs>
</template>
