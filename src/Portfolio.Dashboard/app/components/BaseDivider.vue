<template>
  <div class="relative flex items-center my-6">
    <div :class="['grow border-t', variant === 'glass' ? 'border-secondary-200' : 'border-secondary-200 dark:border-secondary-700']" />
    <span v-if="text" class="font-medium" :class="['shrink mx-4 text-sm', variant === 'glass' ? 'text-secondary-500  px-2' : 'text-secondary-500 dark:text-secondary-400']">
      {{ text }}
    </span>
    <div :class="['grow border-t', variant === 'glass' ? 'border-secondary-200' : 'border-secondary-200 dark:border-secondary-700']" />
  </div>
</template>

<script setup lang="ts">
export interface BaseDividerProps {
  text?: string
  variant?: 'default' | 'glass'
}

withDefaults(defineProps<BaseDividerProps>(), {
  text: '',
  variant: 'default',
})
</script>
