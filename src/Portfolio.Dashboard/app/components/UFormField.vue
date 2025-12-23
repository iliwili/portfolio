<template>
  <div class="space-y-2">
    <Label v-if="label" :for="name" class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">
      {{ label }}
      <span v-if="required" class="text-destructive ml-1">*</span>
    </Label>
    <div class="relative">
      <slot />
    </div>
    <p v-if="hint && !hasError" class="text-[0.8rem] text-muted-foreground">
      {{ hint }}
    </p>
    <p v-if="hasError" class="text-[0.8rem] font-medium text-destructive">
      {{ firstErrorText }}
    </p>
  </div>
</template>

<script setup lang="ts">
import { Label } from '~/components/ui/label'

export interface FormFieldProps {
  name: string
  errors?: ErrorMap
  label?: string
  required?: boolean
  hint?: string
}

const props = withDefaults(defineProps<FormFieldProps>(), {
  errors: undefined,
  label: '',
  required: false,
  hint: '',
})

const { t } = useI18n()

const fieldErrors = computed(() => props.errors?.[props.name] ?? [])

const firstErrorText = computed(() => {
  const e = fieldErrors.value[0]
  return e ? t(e.code, e.args ?? {}) : ''
})

const hasError = computed(() => fieldErrors.value.length > 0)
</script>
