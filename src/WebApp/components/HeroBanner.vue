<template>
  <UPageHero
      title="Just landed."
      description="Our latest collection is here. Discover the latest trends and styles for the new year"
      headline="The New Year Collection"
      :links="[
        {
          label: 'Shop now',
          to: '/products',
        },
      ]"
      orientation="horizontal"
    >
      <NuxtImg
        width="1400"
        height="800"
        class="object-cover w-full h-[420px] lg:h-[560px] xl:h-[640px]"
        src="/images/hero.jpg"
        alt="Hero image"
        loading="eager"
        sizes="sm:100vw md:1400px"
        fetchpriority="high"
        preload
        placeholder
        placeholder-class="blur-xl"
      />
    </UPageHero>
</template>
