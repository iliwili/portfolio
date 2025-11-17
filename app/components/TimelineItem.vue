<template>
  <li class="relative pl-8">
    <span class="absolute left-0 top-2 h-4 w-4 rounded-full bg-accent-400/40 ring-2 ring-accent-400/50" />
    <div class="card">
      <div class="flex flex-wrap items-baseline justify-between gap-2">
        <div>
          <h3 class="font-semibold">
            {{ company }}
          </h3>
          <p class="text-sm text-zinc-300">
            {{ role }}
          </p>
        </div>
        <span class="text-xs text-zinc-400">{{ period }}</span>
      </div>
      <p class="mt-3 text-sm text-zinc-300">
        {{ summary }}
      </p>
    </div>
  </li>
</template>

<script setup lang="ts">
defineProps<{ company: string, role: string, period: string, summary: string }>()
</script>
