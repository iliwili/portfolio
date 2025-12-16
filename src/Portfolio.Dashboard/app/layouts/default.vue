<template>
  <div class="min-h-screen bg-gray-100 dark:bg-gray-950">
    <!-- Sidebar / Navigation -->
    <aside class="fixed inset-y-0 left-0 w-60 bg-gray-900 dark:bg-gray-900 hidden lg:flex lg:flex-col">
      <!-- Account Switcher -->
      <NuxtLink
        to="/accounts"
        class="h-14 flex items-center gap-3 px-4 border-b border-gray-800 hover:bg-gray-800 transition-colors"
      >
        <div class="w-8 h-8 bg-emerald-500 rounded flex items-center justify-center text-white font-semibold text-sm">
          {{ currentAccount ? getAccountInitials(currentAccount.name) : 'P' }}
        </div>
        <div class="flex-1 min-w-0">
          <p class="text-white text-sm font-medium truncate">{{ currentAccount?.name || 'Portfolio' }}</p>
        </div>
        <Icon name="lucide:chevrons-up-down" class="w-4 h-4 text-gray-400" />
      </NuxtLink>

      <!-- Navigation -->
      <nav class="flex-1 p-3 space-y-0.5 overflow-y-auto">
        <NuxtLink
          to="/dashboard"
          class="flex items-center gap-3 px-3 py-2 rounded-lg text-gray-300 hover:text-white hover:bg-gray-800 transition-colors text-sm font-medium"
          active-class="!bg-gray-800 !text-white"
        >
          <Icon name="lucide:home" class="w-5 h-5" />
          <span>Home</span>
        </NuxtLink>
        <NuxtLink
          to="/dashboard/projects"
          class="flex items-center gap-3 px-3 py-2 rounded-lg text-gray-300 hover:text-white hover:bg-gray-800 transition-colors text-sm font-medium"
          active-class="!bg-gray-800 !text-white"
        >
          <Icon name="lucide:folder" class="w-5 h-5" />
          <span>Projects</span>
        </NuxtLink>
        <NuxtLink
          to="/dashboard/analytics"
          class="flex items-center gap-3 px-3 py-2 rounded-lg text-gray-300 hover:text-white hover:bg-gray-800 transition-colors text-sm font-medium"
          active-class="!bg-gray-800 !text-white"
        >
          <Icon name="lucide:bar-chart-2" class="w-5 h-5" />
          <span>Analytics</span>
        </NuxtLink>
        <NuxtLink
          to="/dashboard/settings"
          class="flex items-center gap-3 px-3 py-2 rounded-lg text-gray-300 hover:text-white hover:bg-gray-800 transition-colors text-sm font-medium"
          active-class="!bg-gray-800 !text-white"
        >
          <Icon name="lucide:settings" class="w-5 h-5" />
          <span>Settings</span>
        </NuxtLink>
      </nav>

      <!-- User section at bottom -->
      <div class="p-3 border-t border-gray-800">
        <div class="flex items-center gap-3 px-2 py-1.5 text-sm">
          <div class="w-8 h-8 bg-gray-700 rounded-full flex items-center justify-center">
            <span class="text-white font-medium text-xs">
              {{ user?.firstName?.charAt(0) }}{{ user?.lastName?.charAt(0) }}
            </span>
          </div>
          <div class="flex-1 min-w-0">
            <p class="text-white text-sm font-medium truncate">
              {{ user?.firstName }} {{ user?.lastName }}
            </p>
            <p class="text-gray-400 text-xs truncate">
              {{ user?.email }}
            </p>
          </div>
        </div>
      </div>
    </aside>

    <!-- Main content area -->
    <div class="lg:pl-60">
      <!-- Top bar -->
      <header class="h-14 bg-white dark:bg-gray-900 border-b border-gray-200 dark:border-gray-800 flex items-center justify-between px-4 sticky top-0 z-30">
        <!-- Mobile menu button -->
        <button
          type="button"
          class="lg:hidden p-2 -ml-2 text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800"
          @click="mobileMenuOpen = !mobileMenuOpen"
        >
          <Icon name="lucide:menu" class="w-5 h-5" />
        </button>

        <!-- Search (placeholder) -->
        <div class="hidden md:flex flex-1 max-w-lg ml-4">
          <div class="relative w-full">
            <Icon name="lucide:search" class="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
            <input
              type="text"
              placeholder="Search..."
              class="w-full pl-10 pr-4 py-1.5 text-sm rounded-lg border border-gray-300 dark:border-gray-700 bg-gray-50 dark:bg-gray-800 text-gray-900 dark:text-gray-100 placeholder:text-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 dark:focus:border-blue-400"
            >
          </div>
        </div>

        <!-- Spacer -->
        <div class="flex-1 md:hidden" />

        <!-- Right side -->
        <div class="flex items-center gap-2">
          <!-- Dark mode toggle -->
          <button
            type="button"
            class="p-2 rounded-lg text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
            @click="toggleDarkMode"
          >
            <Icon v-if="colorMode.value === 'dark'" name="lucide:sun" class="w-5 h-5" />
            <Icon v-else name="lucide:moon" class="w-5 h-5" />
          </button>

          <!-- Notifications -->
          <button
            type="button"
            class="p-2 rounded-lg text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors relative"
          >
            <Icon name="lucide:bell" class="w-5 h-5" />
            <span class="absolute top-1.5 right-1.5 w-2 h-2 bg-blue-600 dark:bg-blue-500 rounded-full" />
          </button>

          <!-- User menu -->
          <div class="relative">
            <button
              type="button"
              class="flex items-center gap-2 p-1.5 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
              @click="userMenuOpen = !userMenuOpen"
            >
              <div class="w-8 h-8 bg-gray-200 dark:bg-gray-700 rounded-full flex items-center justify-center">
                <span class="text-gray-600 dark:text-gray-300 font-medium text-sm">
                  {{ user?.firstName?.charAt(0) }}{{ user?.lastName?.charAt(0) }}
                </span>
              </div>
              <Icon name="lucide:chevron-down" class="w-4 h-4 text-gray-400 hidden sm:block" />
            </button>

            <!-- Dropdown menu -->
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
                class="absolute right-0 mt-2 w-56 bg-white dark:bg-gray-800 rounded-lg shadow-lg border border-gray-200 dark:border-gray-700 py-1 z-50"
              >
                <div class="px-4 py-3 border-b border-gray-100 dark:border-gray-700">
                  <p class="text-sm font-medium text-gray-900 dark:text-white">
                    {{ user?.firstName }} {{ user?.lastName }}
                  </p>
                  <p class="text-xs text-gray-500 dark:text-gray-400 truncate">
                    {{ user?.email }}
                  </p>
                </div>
                <NuxtLink
                  to="/dashboard/settings"
                  class="flex items-center gap-2 px-4 py-2 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700"
                  @click="userMenuOpen = false"
                >
                  <Icon name="lucide:settings" class="w-4 h-4" />
                  Settings
                </NuxtLink>
                <NuxtLink
                  to="/dashboard/help"
                  class="flex items-center gap-2 px-4 py-2 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700"
                  @click="userMenuOpen = false"
                >
                  <Icon name="lucide:help-circle" class="w-4 h-4" />
                  Help Center
                </NuxtLink>
                <div class="border-t border-gray-100 dark:border-gray-700 my-1" />
                <button
                  type="button"
                  class="w-full flex items-center gap-2 px-4 py-2 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700"
                  @click="logout"
                >
                  <Icon name="lucide:log-out" class="w-4 h-4" />
                  Log out
                </button>
              </div>
            </Transition>
          </div>
        </div>
      </header>

      <!-- Page content -->
      <main class="p-4 md:p-6">
        <slot />
      </main>
    </div>

    <!-- Click outside handler for user menu -->
    <div v-if="userMenuOpen" class="fixed inset-0 z-40" @click="userMenuOpen = false" />
  </div>
</template>

<script setup lang="ts">
const colorMode = useColorMode()
const { user, logout } = useAuth()
const selectedAccountId = useState<string | null>('auth:selectedAccount', () => null)

const toggleDarkMode = () => {
  colorMode.preference = colorMode.value === 'dark' ? 'light' : 'dark'
}

const userMenuOpen = ref(false)
const mobileMenuOpen = ref(false)

// Get the currently selected account
const currentAccount = computed(() => {
  if (!selectedAccountId.value || !user.value?.accounts) return null
  return user.value.accounts.find(a => a.publicId === selectedAccountId.value)
})

// Get account initials
function getAccountInitials(name: string) {
  return name
    .split(' ')
    .map(word => word.charAt(0))
    .slice(0, 2)
    .join('')
    .toUpperCase()
}
</script>
