<template>
  <button
    :type="type"
    :disabled="disabled || loading"
    :class="[
      'cursor-pointer inline-flex items-center justify-center font-semibold rounded-lg transition-all duration-150',
      'disabled:opacity-50 disabled:cursor-not-allowed disabled:shadow-none',
      variantClasses[variant],
      sizeClasses[size],
      fullWidth ? 'w-full' : '',
    ]"
    @click="$emit('click', $event)"
  >
    <svg
      v-if="loading"
      class="animate-spin -ml-1 mr-2 h-4 w-4"
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
    >
      <circle
        class="opacity-25"
        cx="12"
        cy="12"
        r="10"
        stroke="currentColor"
        stroke-width="4"
      />
      <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
    </svg>
    <slot />
  </button>
</template>

<script setup lang="ts">
export interface BaseButtonProps {
  variant?: 'primary' | 'secondary' | 'outline' | 'ghost' | 'danger' | 'success' | 'outline-glass'
  size?: 'sm' | 'md' | 'lg'
  loading?: boolean
  disabled?: boolean
  fullWidth?: boolean
  type?: 'button' | 'submit' | 'reset'
}

withDefaults(defineProps<BaseButtonProps>(), {
  variant: 'primary',
  size: 'md',
  loading: false,
  disabled: false,
  fullWidth: false,
  type: 'button',
})

defineEmits<{
  click: [event: MouseEvent]
}>()

const variantClasses: Record<string, string> = {
  'primary': 'bg-primary-600 text-white hover:bg-primary-700 active:bg-primary-800 shadow-sm dark:bg-primary-500 dark:hover:bg-primary-600',
  'secondary': 'bg-secondary-100 text-secondary-900 hover:bg-secondary-200 active:bg-secondary-300 dark:bg-secondary-700 dark:text-secondary-100 dark:hover:bg-secondary-600',
  'outline': 'border border-secondary-300 bg-white text-secondary-700 hover:bg-secondary-50 active:bg-secondary-100 dark:border-secondary-600 dark:bg-transparent dark:text-secondary-300 dark:hover:bg-secondary-800',
  'outline-glass': 'border border-secondary-300 bg-white text-secondary-700 hover:bg-secondary-50 active:bg-secondary-100',
  'ghost': 'text-secondary-700 hover:bg-secondary-100 active:bg-secondary-200 dark:text-secondary-300 dark:hover:bg-secondary-800',
  'danger': 'bg-red-600 text-white hover:bg-red-700 active:bg-red-800 shadow-sm',
  'success': 'bg-primary-600 text-white hover:bg-primary-700 active:bg-primary-800 shadow-sm dark:bg-primary-500 dark:hover:bg-primary-600',
}

const sizeClasses: Record<string, string> = {
  sm: 'px-3 py-1.5 text-sm',
  md: 'px-4 py-2 text-sm',
  lg: 'px-5 py-2.5 text-sm',
}
</script>
