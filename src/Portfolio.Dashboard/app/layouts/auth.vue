<template>
  <div class="min-h-screen relative overflow-hidden">
    <!-- Animated gradient background -->
    <div class="absolute inset-0 bg-gradient-to-br from-blue-600 via-purple-500 to-blue-700 animate-gradient-shift" />
    <div class="absolute inset-0 bg-[radial-gradient(ellipse_at_top_left,_rgba(255,255,255,0.15)_0%,_transparent_50%)]" />
    <div class="absolute inset-0 bg-[radial-gradient(ellipse_at_bottom_right,_rgba(0,0,0,0.2)_0%,_transparent_50%)]" />

    <!-- Content -->
    <div class="relative z-10 min-h-screen flex flex-col">
      <!-- Top bar -->
      <header class="h-14 flex justify-between items-center px-6">
        <NuxtLink to="/" class="flex items-center gap-2 text-white">
          <div class="w-8 h-8 bg-white/20 backdrop-blur-sm rounded-lg flex items-center justify-center">
            <Icon name="lucide:briefcase" class="w-4 h-4 text-white" />
          </div>
          <span class="font-semibold text-base">Portfolio</span>
        </NuxtLink>
      </header>

      <!-- Main content -->
      <main class="flex-1 flex items-center justify-center p-4">
        <slot />
      </main>

      <!-- Footer -->
      <footer class="py-6 text-center text-sm text-white/70">
        <div class="flex items-center justify-center gap-4">
          <a href="#" class="hover:text-white transition-colors">Help</a>
          <a href="#" class="hover:text-white transition-colors">Privacy</a>
          <a href="#" class="hover:text-white transition-colors">Terms</a>
        </div>
      </footer>
    </div>
  </div>
</template>

<script setup lang="ts">
</script>

<style scoped>
@keyframes gradient-shift {
  0%, 100% {
    background-size: 200% 200%;
    background-position: 0% 50%;
  }
  50% {
    background-size: 200% 200%;
    background-position: 100% 50%;
  }
}

.animate-gradient-shift {
  animation: gradient-shift 15s ease infinite;
}
</style>
