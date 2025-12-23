<template>
  <div class="min-h-screen bg-background">
    <!-- Top bar -->
    <header class="border-b border-border">
      <div class="container mx-auto px-6 h-16 flex items-center justify-between">
        <NuxtLink to="/" class="flex items-center gap-2 text-foreground hover:text-foreground/80 transition-colors">
          <Icon name="lucide:briefcase" class="w-5 h-5 text-primary" />
          <span class="font-semibold">Portiva</span>
        </NuxtLink>

        <!-- Theme toggle -->
        <button
          type="button"
          class="p-2 rounded-lg text-muted-foreground hover:text-foreground hover:bg-accent transition-colors"
          @click="toggleTheme"
        >
          <Icon v-if="colorMode.value === 'dark'" name="lucide:sun" class="w-5 h-5" />
          <Icon v-else name="lucide:moon" class="w-5 h-5" />
        </button>
      </div>
    </header>

    <!-- Main content -->
    <main class="container mx-auto px-6 py-16 flex items-center justify-center min-h-[calc(100vh-8rem)]">
      <slot />
    </main>

    <!-- Footer -->
    <footer class="border-t border-border">
      <div class="container mx-auto px-6 py-6">
        <div class="flex items-center justify-center gap-6 text-sm text-muted-foreground">
          <a href="#" class="hover:text-foreground transition-colors">Help</a>
          <a href="#" class="hover:text-foreground transition-colors">Privacy</a>
          <a href="#" class="hover:text-foreground transition-colors">Terms</a>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
const colorMode = useColorMode()

function toggleTheme() {
  colorMode.preference = colorMode.value === 'dark' ? 'light' : 'dark'
}
</script>
