<template>
  <article class="bg-zinc-900/70 border border-white/5 rounded-2xl p-5 sm:p-6 backdrop-blur transition-transform duration-300 hover:-translate-y-1 hover:shadow-lg hover:border-blue-500/40">
    <div class="aspect-video w-full rounded-xl bg-gradient-to-br from-white/5 to-white/0 ring-1 ring-inset ring-white/10 mb-4" />
    <h3 class="font-semibold text-lg">
      {{ title }}
    </h3>
    <p class="mt-1 text-sm text-zinc-300">
      {{ description }}
    </p>
    <p class="mt-3 text-xs text-zinc-400">
      {{ tech }}
    </p>
  </article>
</template>

<script setup lang="ts">
defineProps<{
  title: string
  description: string
  tech?: string
}>()
</script>
