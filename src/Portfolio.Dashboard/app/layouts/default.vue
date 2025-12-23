<template>
  <div class="min-h-screen bg-slate-50 dark:bg-slate-950">
    <!-- Sidebar -->
    <aside class="fixed inset-y-0 left-0 w-64 bg-white dark:bg-slate-900 border-r border-slate-200 dark:border-slate-800 hidden lg:flex lg:flex-col">
      <!-- Logo / Account Switcher -->
      <NuxtLink
        to="/accounts"
        class="h-16 flex items-center gap-3 px-6 border-b border-slate-200 dark:border-slate-800 hover:bg-slate-100 dark:hover:bg-slate-800/50 transition-colors"
      >
        <Icon name="lucide:briefcase" class="w-6 h-6 text-primary-600 dark:text-primary-500" />
        <div class="flex-1 min-w-0">
          <p class="text-slate-900 dark:text-slate-100 font-semibold truncate">{{ currentAccount?.name || 'Portfolio' }}</p>
        </div>
        <Icon name="lucide:chevron-down" class="w-4 h-4 text-slate-500 dark:text-slate-400" />
      </NuxtLink>

      <!-- Navigation -->
      <nav class="flex-1 p-4 space-y-1">
        <NuxtLink
          to="/dashboard"
          class="flex items-center gap-3 px-3 py-2 rounded-lg text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-100 hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
          active-class="!bg-slate-100 dark:!bg-slate-800 !text-slate-900 dark:!text-slate-100"
        >
          <Icon name="lucide:layout-dashboard" class="w-5 h-5" />
          <span class="font-medium">Dashboard</span>
        </NuxtLink>
        <NuxtLink
          to="/dashboard/projects"
          class="flex items-center gap-3 px-3 py-2 rounded-lg text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-100 hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
          active-class="!bg-slate-100 dark:!bg-slate-800 !text-slate-900 dark:!text-slate-100"
        >
          <Icon name="lucide:folder" class="w-5 h-5" />
          <span class="font-medium">Projects</span>
        </NuxtLink>
        <NuxtLink
          to="/dashboard/analytics"
          class="flex items-center gap-3 px-3 py-2 rounded-lg text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-100 hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
          active-class="!bg-slate-100 dark:!bg-slate-800 !text-slate-900 dark:!text-slate-100"
        >
          <Icon name="lucide:bar-chart-3" class="w-5 h-5" />
          <span class="font-medium">Analytics</span>
        </NuxtLink>
        <NuxtLink
          to="/dashboard/settings"
          class="flex items-center gap-3 px-3 py-2 rounded-lg text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-100 hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
          active-class="!bg-slate-100 dark:!bg-slate-800 !text-slate-900 dark:!text-slate-100"
        >
          <Icon name="lucide:settings" class="w-5 h-5" />
          <span class="font-medium">Settings</span>
        </NuxtLink>
      </nav>

      <!-- User section -->
      <div class="p-4 border-t border-slate-200 dark:border-slate-800">
        <button
          type="button"
          class="w-full flex items-center gap-3 px-3 py-2 rounded-lg hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors text-left"
          @click="handleLogout"
        >
          <div class="w-8 h-8 bg-slate-200 dark:bg-slate-800 rounded-full flex items-center justify-center">
            <span class="text-slate-700 dark:text-slate-200 font-medium text-sm">
              {{ user?.firstName?.charAt(0) }}{{ user?.lastName?.charAt(0) }}
            </span>
          </div>
          <div class="flex-1 min-w-0">
            <p class="text-slate-900 dark:text-slate-100 font-medium text-sm truncate">
              {{ user?.firstName }} {{ user?.lastName }}
            </p>
            <p class="text-slate-500 dark:text-slate-400 text-xs truncate">
              {{ user?.email }}
            </p>
          </div>
          <Icon name="lucide:log-out" class="w-4 h-4 text-slate-500 dark:text-slate-400" />
        </button>
      </div>
    </aside>

    <!-- Main content -->
    <div class="lg:pl-64">
      <!-- Top bar -->
      <header class="h-16 bg-white dark:bg-slate-900 border-b border-slate-200 dark:border-slate-800 flex items-center justify-between px-6 sticky top-0 z-30">
        <!-- Mobile menu -->
        <button
          type="button"
          class="lg:hidden p-2 text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-100 rounded-lg hover:bg-slate-100 dark:hover:bg-slate-800"
          @click="mobileMenuOpen = !mobileMenuOpen"
        >
          <Icon name="lucide:menu" class="w-5 h-5" />
        </button>

        <!-- Search -->
        <div class="hidden md:flex flex-1 max-w-lg">
          <div class="relative w-full">
            <Icon name="lucide:search" class="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />
            <input
              type="text"
              placeholder="Search..."
              class="w-full pl-10 pr-4 py-2 rounded-lg border border-slate-300 dark:border-slate-800 bg-white dark:bg-slate-950 text-slate-900 dark:text-slate-100 placeholder:text-slate-500 focus:outline-none focus:ring-2 focus:ring-primary-600 focus:border-transparent"
            >
          </div>
        </div>

        <!-- Right side -->
        <div class="flex items-center gap-2">
          <!-- Theme toggle -->
          <button
            type="button"
            class="p-2 rounded-lg text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-100 hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
            @click="toggleTheme"
          >
            <Icon v-if="colorMode.value === 'dark'" name="lucide:sun" class="w-5 h-5" />
            <Icon v-else name="lucide:moon" class="w-5 h-5" />
          </button>
          
          <!-- Notifications -->
          <button
            type="button"
            class="p-2 rounded-lg text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-100 hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors relative"
          >
            <Icon name="lucide:bell" class="w-5 h-5" />
            <span class="absolute top-2 right-2 w-2 h-2 bg-primary-600 dark:bg-primary-500 rounded-full" />
          </button>

          <!-- Help -->
          <button
            type="button"
            class="p-2 rounded-lg text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-100 hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
          >
            <Icon name="lucide:help-circle" class="w-5 h-5" />
          </button>

          <!-- User avatar -->
          <button
            type="button"
            class="flex items-center gap-2 p-1 rounded-lg hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
            @click="userMenuOpen = !userMenuOpen"
          >
            <div class="w-8 h-8 bg-slate-200 dark:bg-slate-800 rounded-full flex items-center justify-center">
              <span class="text-slate-700 dark:text-slate-200 font-medium text-sm">
                {{ user?.firstName?.charAt(0) }}{{ user?.lastName?.charAt(0) }}
              </span>
            </div>
          </button>

          <!-- User dropdown -->
          <Transition
            enter-active-class="transition duration-100 ease-out"
            enter-from-class="opacity-0 scale-95"
            enter-to-class="opacity-100 scale-100"
            leave-active-class="transition duration-75 ease-in"
            leave-from-class="opacity-100 scale-100"
            leave-to-class="opacity-0 scale-95"
          >
            <div
              v-if="userMenuOpen"
              class="absolute right-6 top-14 mt-2 w-56 bg-white dark:bg-slate-900 rounded-lg shadow-xl border border-slate-200 dark:border-slate-800 py-1 z-50"
            >
              <div class="px-4 py-3 border-b border-slate-200 dark:border-slate-800">
                <p class="text-sm font-medium text-slate-900 dark:text-slate-100">
                  {{ user?.firstName }} {{ user?.lastName }}
                </p>
                <p class="text-xs text-slate-500 dark:text-slate-400 truncate">
                  {{ user?.email }}
                </p>
              </div>
              <NuxtLink
                to="/dashboard/settings"
                class="flex items-center gap-2 px-4 py-2 text-sm text-slate-700 dark:text-slate-300 hover:bg-slate-100 dark:hover:bg-slate-800"
                @click="userMenuOpen = false"
              >
                <Icon name="lucide:settings" class="w-4 h-4" />
                Settings
              </NuxtLink>
              <div class="border-t border-slate-200 dark:border-slate-800 my-1" />
              <button
                type="button"
                class="w-full flex items-center gap-2 px-4 py-2 text-sm text-slate-700 dark:text-slate-300 hover:bg-slate-100 dark:hover:bg-slate-800"
                @click="handleLogout"
              >
                <Icon name="lucide:log-out" class="w-4 h-4" />
                Log out
              </button>
            </div>
          </Transition>
        </div>
      </header>

      <!-- Page content -->
      <main class="p-6">
        <slot />
      </main>
    </div>

    <!-- Click outside handler -->
    <div v-if="userMenuOpen" class="fixed inset-0 z-40" @click="userMenuOpen = false" />
  </div>
</template>

<script setup lang="ts">
const colorMode = useColorMode()
const { user, logout } = useAuth()
const selectedAccountId = useState<string | null>('auth:selectedAccount', () => null)

const userMenuOpen = ref(false)
const mobileMenuOpen = ref(false)

function toggleTheme() {
  colorMode.preference = colorMode.value === 'dark' ? 'light' : 'dark'
}

// Get the currently selected account
const currentAccount = computed(() => {
  if (!selectedAccountId.value || !user.value?.accounts) return null
  return user.value.accounts.find(a => a.publicId === selectedAccountId.value)
})

// Logout handler
async function handleLogout() {
  userMenuOpen.value = false
  await logout()
}
</script>
