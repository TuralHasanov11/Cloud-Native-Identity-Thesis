<template>
  <form v-if="customer" class="bg-white rounded-lg shadow">
    <div class="grid gap-6 p-8 md:grid-cols-2">
      <h3 class="text-xl font-semibold col-span-full">
        {{ $t("messages.account.personalInfo") }}
      </h3>

      {{ customer.name }}

      {{ user?.name }}

      {{ user?.email }}
    </div>
  </form>
</template>

<script setup lang="ts">
const { customer } = useCustomer();
const { user } = useUserSession();
</script>
