<script setup lang="ts">

</script>
<template>
  <section>
    <section class="bg-center bg-cover bg-no-repeat blur-none z-0" style="background-image: url('/images/hero.jpg')">
      <div class="pb-36 pt-2 px-6 relative rounded-3xl md:pb-48 lg:pb-72 lg:px-12">
        <div class="-mx-4 flex flex-wrap items-center justify-center mt-24 space-y-6 lg:space-y-0">
          <div class="px-4 text-center w-full md:w-8/12 xl:w-6/12">
            <h3 class="dark:text-secondary-200 font-bold font-serif lg:capitalize mb-1 text-primary-600 uppercase">
              New Arrivals, Big Savings!
            </h3>
            <UButton label="Shop Now" :to="{ name: 'products' }" size="xl" trailing class="mt-4" />
          </div>
        </div>
      </div>
    </section>
  </section>
</template>
