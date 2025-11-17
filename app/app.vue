<template>
  <div>
    <!-- Subtle background grid / glow -->
    <div class="pointer-events-none fixed inset-0 -z-10">
      <div class="absolute inset-0 opacity-[0.06]" :style="gridStyle" />
      <div
        class="absolute -top-40 left-1/2 -translate-x-1/2 h-72 w-[42rem] rounded-full blur-3xl opacity-30"
        style="background: radial-gradient(closest-side, rgba(59,130,246,.35), transparent 70%);"
      />
    </div>
    <NuxtPage />
  </div>
</template>

<script setup lang="ts">
const gridStyle = `background-image: linear-gradient(rgba(255,255,255,.04) 1px, transparent 1px), linear-gradient(90deg, rgba(255,255,255,.04) 1px, transparent 1px); background-size: 24px 24px;`
</script>
