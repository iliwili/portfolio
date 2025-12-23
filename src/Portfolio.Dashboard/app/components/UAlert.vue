<template>
  <div
    :class="[
      'p-4 rounded-lg border',
      variantClasses[variant],
    ]"
  >
    <div class="flex items-start gap-3">
      <Icon :name="iconName" class="w-5 h-5 shrink-0 mt-0.5" />
      <div class="flex-1">
        <slot />
      </div>
      <button
        v-if="dismissible"
        type="button"
        class="text-current opacity-50 hover:opacity-100 transition-opacity"
        @click="$emit('dismiss')"
      >
        <Icon name="lucide:x" class="w-4 h-4" />
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
export interface UAlertProps {
  variant?: 'info' | 'success' | 'warning' | 'error'
  dismissible?: boolean
}

const props = withDefaults(defineProps<UAlertProps>(), {
  variant: 'info',
  dismissible: false,
})

defineEmits<{
  dismiss: []
}>()

const variantClasses: Record<string, string> = {
  info: 'bg-primary-50 border-primary-200 text-primary-900 dark:bg-primary-950/50 dark:border-primary-800/50 dark:text-primary-200',
  success: 'bg-green-50 border-green-200 text-green-900 dark:bg-green-950/50 dark:border-green-800/50 dark:text-green-200',
  warning: 'bg-yellow-50 border-yellow-200 text-yellow-900 dark:bg-yellow-950/50 dark:border-yellow-800/50 dark:text-yellow-200',
  error: 'bg-red-50 border-red-200 text-red-900 dark:bg-red-950/50 dark:border-red-800/50 dark:text-red-200',
}

const iconName = computed(() => {
  const icons = {
    info: 'lucide:info',
    success: 'lucide:check-circle',
    warning: 'lucide:alert-triangle',
    error: 'lucide:x-circle',
  }
  return icons[props.variant]
})
</script>
