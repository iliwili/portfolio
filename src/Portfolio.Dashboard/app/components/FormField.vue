<script setup lang="ts">
type ApiValidationError = { code: string, args?: any }
type ErrorMap = Record<string, ApiValidationError[]>

export interface FormFieldProps {
  name: string
  errors?: ErrorMap
  label?: string
  required?: boolean
  hint?: string
  variant?: 'default' | 'glass'
}

const props = withDefaults(defineProps<FormFieldProps>(), {
  errors: undefined,
  label: '',
  required: false,
  hint: '',
  variant: 'default',
})

const isGlass = computed(() => props.variant === 'glass')

const { t } = useI18n()

const fieldErrors = computed(() => props.errors?.[props.name] ?? [])

const firstErrorText = computed(() => {
  const e = fieldErrors.value[0]
  return e ? t(e.code, e.args ?? {}) : ''
})

const hasError = computed(() => fieldErrors.value.length > 0)
</script>

<template>
  <div class="flex flex-col gap-1.5">
    <label
      v-if="label"
      :class="[
        'text-sm font-medium',
        isGlass ? 'text-secondary-700' : 'text-secondary-800 dark:text-secondary-200',
      ]"
    >
      {{ label }}
      <span v-if="required" class="text-red-500">*</span>
    </label>

    <!-- Slot: you render any input you want -->
    <div :data-invalid="hasError">
      <slot :invalid="hasError" />
    </div>

    <p v-if="hint && !hasError" :class="['text-xs', isGlass ? 'text-secondary-500' : 'text-secondary-500 dark:text-secondary-400']">
      {{ hint }}
    </p>

    <p v-if="hasError" class="text-xs text-red-500 flex items-center gap-1">
      <Icon name="lucide:alert-circle" class="w-3.5 h-3.5" />
      {{ firstErrorText }}
    </p>
  </div>
</template>
