<template>
  <span
    class="group relative inline-flex items-center gap-1.5 px-3 py-1.5 bg-gradient-to-r from-zinc-800/50 to-zinc-700/30 border border-zinc-600/40 rounded-lg text-xs font-medium text-zinc-200 shadow-lg backdrop-blur-sm transition-all duration-200 hover:scale-105 hover:shadow-accent-400/20 hover:border-accent-400/50 hover:bg-gradient-to-r hover:from-accent-400/10 hover:to-blue-400/10"
  >
    <!-- Tech icon placeholder -->
    <div class="w-1.5 h-1.5 rounded-full bg-gradient-to-r from-accent-400 to-blue-400 group-hover:animate-pulse" />
    {{ label }}
    <!-- Subtle glow effect -->
    <div class="absolute inset-0 rounded-xl bg-gradient-to-r from-accent-400/0 via-accent-400/5 to-blue-400/0 opacity-0 group-hover:opacity-100 transition-opacity duration-200" />
  </span>
</template>

<script setup lang="ts">
defineProps<{ label: string }>()
</script>
