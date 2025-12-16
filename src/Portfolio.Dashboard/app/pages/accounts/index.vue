<template>
  <div class="min-h-screen flex items-center justify-center p-4 bg-gradient-shopify">
    <!-- Account Chooser Card -->
    <div class="w-full max-w-md bg-white rounded-xl shadow-2xl overflow-hidden">
      <!-- Header -->
      <div class="p-6 pb-4">
        <div class="flex items-center justify-between mb-6">
          <!-- Logo -->
          <div class="flex items-center gap-2">
            <div class="w-8 h-8 bg-blue-600 dark:bg-blue-500 rounded flex items-center justify-center">
              <Icon name="lucide:briefcase" class="w-4 h-4 text-white" />
            </div>
            <span class="font-semibold text-gray-900 dark:text-gray-100">Portfolio</span>
          </div>

          <!-- User Avatar -->
          <div class="w-9 h-9 bg-blue-600 dark:bg-blue-500 rounded-full flex items-center justify-center">
            <span class="text-white font-semibold text-sm">{{ userInitials }}</span>
          </div>
        </div>

        <!-- Welcome Message -->
        <div class="flex items-center justify-between">
          <h1 class="text-xl font-semibold text-gray-900">
            Welcome back, {{ user?.firstName }}
          </h1>
          <button
            type="button"
            class="inline-flex items-center gap-1.5 px-3 py-1.5 bg-gray-900 hover:bg-gray-800 text-white text-sm font-medium rounded-lg transition-colors"
          >
            <Icon name="lucide:plus" class="w-4 h-4" />
            Create store
          </button>
        </div>
      </div>

      <!-- Tabs -->
      <div class="px-6 border-b border-gray-200">
        <div class="flex gap-6">
          <button
            type="button"
            class="pb-3 text-sm font-medium transition-colors relative"
            :class="activeTab === 'active'
              ? 'text-gray-900'
              : 'text-gray-500 hover:text-gray-700'"
            @click="activeTab = 'active'"
          >
            Active
            <span
              v-if="activeTab === 'active'"
              class="absolute bottom-0 left-0 right-0 h-0.5 bg-gray-900 rounded-full"
            />
          </button>
          <button
            type="button"
            class="pb-3 text-sm font-medium transition-colors relative"
            :class="activeTab === 'inactive'
              ? 'text-gray-900'
              : 'text-gray-500 hover:text-gray-700'"
            @click="activeTab = 'inactive'"
          >
            Inactive
            <span
              v-if="activeTab === 'inactive'"
              class="absolute bottom-0 left-0 right-0 h-0.5 bg-gray-900 rounded-full"
            />
          </button>
        </div>
      </div>

      <!-- Account List -->
      <div class="p-4">
        <!-- Active accounts -->
        <div v-if="activeTab === 'active'" class="space-y-2">
          <template v-if="activeAccounts.length > 0">
            <button
              v-for="account in activeAccounts"
              :key="account.publicId"
              type="button"
              class="w-full flex items-center gap-3 p-3 rounded-lg hover:bg-gray-50 transition-colors text-left group"
              @click="selectAccount(account.publicId)"
            >
              <!-- Account Avatar -->
              <div
                :class="[
                  'w-10 h-10 rounded-lg flex items-center justify-center text-white font-semibold text-sm',
                  getAccountColor(account.name),
                ]"
              >
                {{ getAccountInitials(account.name) }}
              </div>

              <!-- Account Info -->
              <div class="flex-1 min-w-0">
                <p class="font-medium text-gray-900 truncate">
                  {{ account.name }}
                </p>
                <p class="text-sm text-gray-500 truncate">
                  {{ account.slug }}.portfolio.app
                </p>
              </div>

              <!-- Arrow -->
              <Icon
                name="lucide:chevron-right"
                class="w-5 h-5 text-gray-400 opacity-0 group-hover:opacity-100 transition-opacity"
              />
            </button>
          </template>

          <!-- Empty state -->
          <div v-else class="py-8 text-center">
            <div class="w-12 h-12 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-3">
              <Icon name="lucide:folder" class="w-6 h-6 text-gray-400" />
            </div>
            <p class="text-gray-500 dark:text-gray-400 text-sm">
              No active stores
            </p>
            <button
              type="button"
              class="mt-3 text-blue-600 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300 text-sm font-medium"
            >
              Create your first store
            </button>
          </div>
        </div>

        <!-- Inactive accounts -->
        <div v-if="activeTab === 'inactive'" class="space-y-2">
          <template v-if="inactiveAccounts.length > 0">
            <button
              v-for="account in inactiveAccounts"
              :key="account.publicId"
              type="button"
              class="w-full flex items-center gap-3 p-3 rounded-lg hover:bg-gray-50 transition-colors text-left group opacity-60"
              @click="selectAccount(account.publicId)"
            >
              <div
                :class="[
                  'w-10 h-10 rounded-lg flex items-center justify-center text-white font-semibold text-sm',
                  getAccountColor(account.name),
                ]"
              >
                {{ getAccountInitials(account.name) }}
              </div>
              <div class="flex-1 min-w-0">
                <p class="font-medium text-gray-900 truncate">
                  {{ account.name }}
                </p>
                <p class="text-sm text-gray-500 truncate">
                  {{ account.slug }}.portfolio.app
                </p>
              </div>
            </button>
          </template>

          <!-- Empty state -->
          <div v-else class="py-8 text-center">
            <p class="text-gray-500 text-sm">
              No inactive stores
            </p>
          </div>
        </div>
      </div>

      <!-- Footer -->
      <div class="px-6 py-4 border-t border-gray-100 dark:border-gray-800 bg-gray-50 dark:bg-gray-800">
        <div class="flex items-center justify-between text-sm">
          <span class="text-gray-500 dark:text-gray-400">Logged in as {{ user?.email }}</span>
          <NuxtLink
            to="/auth/login"
            class="text-blue-600 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300 font-medium"
          >
            Log out
          </NuxtLink>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: false,
})

const { user } = useAuth()
const selectedAccount = useState<string | null>('auth:selectedAccount', () => null)

// Get user initials for avatar
const userInitials = computed(() => {
  if (!user.value) return ''
  return `${user.value.firstName?.charAt(0) || ''}${user.value.lastName?.charAt(0) || ''}`
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

// Generate a consistent color for account avatar based on name
function getAccountColor(name: string) {
  const colors = [
    'bg-emerald-500',
    'bg-blue-500',
    'bg-purple-500',
    'bg-pink-500',
    'bg-orange-500',
    'bg-cyan-500',
    'bg-rose-500',
    'bg-indigo-500',
  ]
  let hash = 0
  for (let i = 0; i < name.length; i++) {
    hash = name.charCodeAt(i) + ((hash << 5) - hash)
  }
  return colors[Math.abs(hash) % colors.length]
}

// Select account and navigate to dashboard
function selectAccount(accountId: string) {
  selectedAccount.value = accountId
  navigateTo('/dashboard')
}

// Active/Inactive tabs
const activeTab = ref<'active' | 'inactive'>('active')

// For now, all accounts are active (you can add status to AccountMembershipDto later)
const activeAccounts = computed(() => user.value?.accounts || [])
const inactiveAccounts = computed(() => [] as typeof activeAccounts.value)
</script>

<style scoped>
.bg-gradient-shopify {
  background: linear-gradient(
    135deg,
    #1a1a2e 0%,
    #16213e 25%,
    #0f3460 50%,
    #1a535c 75%,
    #2d6a6a 100%
  );
}
</style>
