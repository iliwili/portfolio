<template>
  <div class="max-w-7xl">
    <!-- Page header -->
    <div class="mb-6">
      <h1 class="text-xl font-semibold text-gray-900 dark:text-white">
        Good morning, {{ user?.firstName }}
      </h1>
      <p class="text-gray-500 dark:text-gray-400 text-sm mt-0.5">
        Here's what's happening with your portfolio today.
      </p>
    </div>

    <!-- Stats cards -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
      <BaseCard padding="md">
        <div class="flex items-start justify-between">
          <div>
            <p class="text-sm font-medium text-gray-500 dark:text-gray-400">
              Total Projects
            </p>
            <p class="text-2xl font-semibold text-gray-900 dark:text-white mt-1">
              0
            </p>
            <p class="text-xs text-gray-500 dark:text-gray-400 mt-1">
              <span class="text-[#008060]">↑ 0%</span> from last month
            </p>
          </div>
          <div class="w-10 h-10 bg-gray-100 dark:bg-gray-700 rounded-lg flex items-center justify-center">
            <Icon name="lucide:folder" class="w-5 h-5 text-gray-600 dark:text-gray-300" />
          </div>
        </div>
      </BaseCard>

      <BaseCard padding="md">
        <div class="flex items-start justify-between">
          <div>
            <p class="text-sm font-medium text-gray-500 dark:text-gray-400">
              Total Views
            </p>
            <p class="text-2xl font-semibold text-gray-900 dark:text-white mt-1">
              0
            </p>
            <p class="text-xs text-gray-500 dark:text-gray-400 mt-1">
              <span class="text-[#008060]">↑ 0%</span> from last month
            </p>
          </div>
          <div class="w-10 h-10 bg-gray-100 dark:bg-gray-700 rounded-lg flex items-center justify-center">
            <Icon name="lucide:eye" class="w-5 h-5 text-gray-600 dark:text-gray-300" />
          </div>
        </div>
      </BaseCard>

      <BaseCard padding="md">
        <div class="flex items-start justify-between">
          <div>
            <p class="text-sm font-medium text-gray-500 dark:text-gray-400">
              Unique Visitors
            </p>
            <p class="text-2xl font-semibold text-gray-900 dark:text-white mt-1">
              0
            </p>
            <p class="text-xs text-gray-500 dark:text-gray-400 mt-1">
              <span class="text-[#008060]">↑ 0%</span> from last month
            </p>
          </div>
          <div class="w-10 h-10 bg-gray-100 dark:bg-gray-700 rounded-lg flex items-center justify-center">
            <Icon name="lucide:users" class="w-5 h-5 text-gray-600 dark:text-gray-300" />
          </div>
        </div>
      </BaseCard>

      <BaseCard padding="md">
        <div class="flex items-start justify-between">
          <div>
            <p class="text-sm font-medium text-gray-500 dark:text-gray-400">
              Messages
            </p>
            <p class="text-2xl font-semibold text-gray-900 dark:text-white mt-1">
              0
            </p>
            <p class="text-xs text-gray-500 dark:text-gray-400 mt-1">
              <span class="text-amber-500">0 unread</span>
            </p>
          </div>
          <div class="w-10 h-10 bg-gray-100 dark:bg-gray-700 rounded-lg flex items-center justify-center">
            <Icon name="lucide:mail" class="w-5 h-5 text-gray-600 dark:text-gray-300" />
          </div>
        </div>
      </BaseCard>
    </div>

    <!-- Main content grid -->
    <div class="grid grid-cols-1 lg:grid-cols-3 gap-4">
      <!-- Getting started card -->
      <div class="lg:col-span-2">
        <BaseCard padding="md">
          <div class="flex items-center justify-between mb-4">
            <h2 class="text-base font-semibold text-gray-900 dark:text-white">
              Getting started
            </h2>
            <span class="text-xs text-gray-500 dark:text-gray-400">0 of 4 completed</span>
          </div>

          <div class="space-y-3">
            <div class="flex items-center gap-3 p-3 rounded-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-800/50 transition-colors cursor-pointer">
              <div class="w-8 h-8 border-2 border-gray-300 dark:border-gray-600 rounded-full flex items-center justify-center">
                <Icon name="lucide:check" class="w-4 h-4 text-gray-300 dark:text-gray-600" />
              </div>
              <div class="flex-1">
                <p class="text-sm font-medium text-gray-900 dark:text-white">
                  Create your first project
                </p>
                <p class="text-xs text-gray-500 dark:text-gray-400">
                  Add a project to showcase your work
                </p>
              </div>
              <Icon name="lucide:chevron-right" class="w-4 h-4 text-gray-400" />
            </div>

            <div class="flex items-center gap-3 p-3 rounded-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-800/50 transition-colors cursor-pointer">
              <div class="w-8 h-8 border-2 border-gray-300 dark:border-gray-600 rounded-full flex items-center justify-center">
                <Icon name="lucide:check" class="w-4 h-4 text-gray-300 dark:text-gray-600" />
              </div>
              <div class="flex-1">
                <p class="text-sm font-medium text-gray-900 dark:text-white">
                  Complete your profile
                </p>
                <p class="text-xs text-gray-500 dark:text-gray-400">
                  Add your bio, skills, and social links
                </p>
              </div>
              <Icon name="lucide:chevron-right" class="w-4 h-4 text-gray-400" />
            </div>

            <div class="flex items-center gap-3 p-3 rounded-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-800/50 transition-colors cursor-pointer">
              <div class="w-8 h-8 border-2 border-gray-300 dark:border-gray-600 rounded-full flex items-center justify-center">
                <Icon name="lucide:check" class="w-4 h-4 text-gray-300 dark:text-gray-600" />
              </div>
              <div class="flex-1">
                <p class="text-sm font-medium text-gray-900 dark:text-white">
                  Customize your theme
                </p>
                <p class="text-xs text-gray-500 dark:text-gray-400">
                  Choose colors and layout for your portfolio
                </p>
              </div>
              <Icon name="lucide:chevron-right" class="w-4 h-4 text-gray-400" />
            </div>

            <div class="flex items-center gap-3 p-3 rounded-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-800/50 transition-colors cursor-pointer">
              <div class="w-8 h-8 border-2 border-gray-300 dark:border-gray-600 rounded-full flex items-center justify-center">
                <Icon name="lucide:check" class="w-4 h-4 text-gray-300 dark:text-gray-600" />
              </div>
              <div class="flex-1">
                <p class="text-sm font-medium text-gray-900 dark:text-white">
                  Share your portfolio
                </p>
                <p class="text-xs text-gray-500 dark:text-gray-400">
                  Get your public link and start sharing
                </p>
              </div>
              <Icon name="lucide:chevron-right" class="w-4 h-4 text-gray-400" />
            </div>
          </div>
        </BaseCard>
      </div>

      <!-- Quick actions -->
      <div>
        <BaseCard padding="md">
          <h2 class="text-base font-semibold text-gray-900 dark:text-white mb-4">
            Quick actions
          </h2>
          <div class="space-y-2">
            <button
              type="button"
              class="w-full flex items-center gap-3 p-3 rounded-lg bg-blue-600 hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600 text-white transition-colors text-left"
            >
              <Icon name="lucide:plus" class="w-5 h-5" />
              <span class="text-sm font-medium">New Project</span>
            </button>

            <button
              type="button"
              class="w-full flex items-center gap-3 p-3 rounded-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-800/50 transition-colors text-left"
            >
              <Icon name="lucide:user" class="w-5 h-5 text-gray-600 dark:text-gray-400" />
              <span class="text-sm font-medium text-gray-900 dark:text-white">Edit Profile</span>
            </button>

            <button
              type="button"
              class="w-full flex items-center gap-3 p-3 rounded-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-800/50 transition-colors text-left"
            >
              <Icon name="lucide:external-link" class="w-5 h-5 text-gray-600 dark:text-gray-400" />
              <span class="text-sm font-medium text-gray-900 dark:text-white">View Portfolio</span>
            </button>

            <button
              type="button"
              class="w-full flex items-center gap-3 p-3 rounded-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-800/50 transition-colors text-left"
            >
              <Icon name="lucide:share-2" class="w-5 h-5 text-gray-600 dark:text-gray-400" />
              <span class="text-sm font-medium text-gray-900 dark:text-white">Share Link</span>
            </button>
          </div>
        </BaseCard>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const { user } = useAuth()
</script>
