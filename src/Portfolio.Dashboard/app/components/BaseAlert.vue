<template>
  <Transition
    enter-active-class="transition duration-200 ease-out"
    enter-from-class="opacity-0 scale-95"
    enter-to-class="opacity-100 scale-100"
    leave-active-class="transition duration-150 ease-in"
    leave-from-class="opacity-100 scale-100"
    leave-to-class="opacity-0 scale-95"
  >
    <div
      v-if="visible"
      :class="[
        'rounded-lg border px-3 py-2.5',
        glass ? config?.bgGlass : config?.bg,
        glass ? config?.borderGlass : config?.border,
        glass ? config?.textGlass : config?.text,
      ]"
      role="alert"
    >
      <div class="flex items-start gap-2.5">
        <Icon :name="config?.icon ?? ''" class="w-4 h-4 shrink-0 mt-0.5" />
        <div class="flex-1 min-w-0 text-sm">
          <p v-if="title" class="font-medium">
            {{ title }}
          </p>
          <div :class="title ? 'mt-0.5 opacity-90' : ''">
            <slot />
          </div>
        </div>
        <button
          v-if="dismissible"
          type="button"
          class="shrink-0 hover:opacity-70 transition-opacity -mt-0.5"
          @click="dismiss"
        >
          <Icon name="lucide:x" class="w-3.5 h-3.5" />
        </button>
      </div>
    </div>
  </Transition>
</template>

<script setup lang="ts">
export interface BaseAlertProps {
  variant?: 'info' | 'success' | 'warning' | 'error'
  title?: string
  dismissible?: boolean
  glass?: boolean
}

const props = withDefaults(defineProps<BaseAlertProps>(), {
  variant: 'info',
  title: '',
  dismissible: false,
  glass: false,
})

const emit = defineEmits<{
  dismiss: []
}>()

const visible = ref(true)

const variantConfig: Record<string, { bg: string, bgGlass: string, border: string, borderGlass: string, text: string, textGlass: string, icon: string }> = {
  info: {
    bg: 'bg-sky-50 dark:bg-sky-900/20',
    bgGlass: 'bg-sky-50',
    border: 'border-sky-200 dark:border-sky-800',
    borderGlass: 'border-sky-200',
    text: 'text-sky-800 dark:text-sky-200',
    textGlass: 'text-sky-800',
    icon: 'lucide:info',
  },
  success: {
    bg: 'bg-emerald-50 dark:bg-emerald-900/20',
    bgGlass: 'bg-emerald-50',
    border: 'border-emerald-200 dark:border-emerald-800',
    borderGlass: 'border-emerald-200',
    text: 'text-emerald-800 dark:text-emerald-200',
    textGlass: 'text-emerald-700',
    icon: 'lucide:check-circle',
  },
  warning: {
    bg: 'bg-amber-50 dark:bg-amber-900/20',
    bgGlass: 'bg-amber-50',
    border: 'border-amber-200 dark:border-amber-800',
    borderGlass: 'border-amber-200',
    text: 'text-amber-800 dark:text-amber-200',
    textGlass: 'text-amber-800',
    icon: 'lucide:alert-triangle',
  },
  error: {
    bg: 'bg-red-50 dark:bg-red-900/20',
    bgGlass: 'bg-red-50',
    border: 'border-red-200 dark:border-red-800',
    borderGlass: 'border-red-200',
    text: 'text-red-700 dark:text-red-200',
    textGlass: 'text-red-700',
    icon: 'lucide:x-circle',
  },
}

const config = computed(() => variantConfig[props.variant] ?? variantConfig.info)

function dismiss() {
  visible.value = false
  emit('dismiss')
}
</script>
