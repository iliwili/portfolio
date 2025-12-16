<template>
  <NuxtLink
    v-if="to && !external"
    :to="to"
    :class="[
      'font-medium transition-colors hover:underline',
      variant === 'glass' ? 'text-primary-600 hover:text-primary-700' : 'text-primary-600 hover:text-primary-700 dark:text-primary-400 dark:hover:text-primary-300',
    ]"
  >
    <slot />
  </NuxtLink>
  <a
    v-else
    :href="href || to"
    :target="external ? '_blank' : undefined"
    :rel="external ? 'noopener noreferrer' : undefined"
    :class="[
      'font-medium transition-colors hover:underline inline-flex items-center gap-1',
      variant === 'glass' ? 'text-primary-600 hover:text-primary-700' : 'text-primary-600 hover:text-primary-700 dark:text-primary-400 dark:hover:text-primary-300',
    ]"
  >
    <slot />
    <Icon v-if="external" name="lucide:external-link" class="w-3.5 h-3.5" />
  </a>
</template>

<script setup lang="ts">
export interface BaseLinkProps {
  to?: string
  href?: string
  external?: boolean
  variant?: 'default' | 'glass'
}

withDefaults(defineProps<BaseLinkProps>(), {
  to: '',
  href: '',
  external: false,
  variant: 'default',
})
</script>
