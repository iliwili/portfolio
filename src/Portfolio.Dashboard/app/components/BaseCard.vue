<template>
  <div
    :class="[
      'rounded-2xl',
      variantClasses[variant],
      paddingClasses[padding],
    ]"
  >
    <slot />
  </div>
</template>

<script setup lang="ts">
export interface BaseCardProps {
  padding?: 'none' | 'sm' | 'md' | 'lg'
  variant?: 'default' | 'glass'
}

withDefaults(defineProps<BaseCardProps>(), {
  padding: 'lg',
  variant: 'default',
})

const paddingClasses: Record<string, string> = {
  none: '',
  sm: 'p-4',
  md: 'p-6',
  lg: 'p-8',
}

const variantClasses: Record<string, string> = {
  default: 'bg-white dark:bg-secondary-800 border border-secondary-200 dark:border-secondary-700 shadow-sm',
  glass: 'bg-white/95 backdrop-blur-sm shadow-2xl',
}
</script>
