<script setup lang="ts">
import { SelectContent, SelectIcon, SelectItem, SelectItemIndicator, SelectItemText, SelectPortal, SelectRoot, SelectTrigger, SelectValue, SelectViewport } from 'reka-ui'

export interface BaseSelectOption {
  value: string
  label: string
  disabled?: boolean
}

export interface BaseSelectProps {
  modelValue?: string
  options: BaseSelectOption[]
  placeholder?: string
  label?: string
  disabled?: boolean
  required?: boolean
  error?: string
  variant?: 'default' | 'glass'
}

const props = withDefaults(defineProps<BaseSelectProps>(), {
  modelValue: '',
  placeholder: 'Select an option',
  label: '',
  disabled: false,
  required: false,
  error: '',
  variant: 'default',
})

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()

const isGlass = computed(() => props.variant === 'glass')
</script>

<template>
  <div class="flex flex-col gap-1.5">
    <label
      v-if="label"
      :class="[
        'text-sm font-medium',
        isGlass ? 'text-gray-700' : 'text-gray-800 dark:text-gray-200',
      ]"
    >
      {{ label }}
      <span v-if="required" class="text-red-500">*</span>
    </label>

    <SelectRoot
      :model-value="modelValue"
      :disabled="disabled"
      @update:model-value="emit('update:modelValue', $event)"
    >
      <SelectTrigger
        :class="[
          'inline-flex items-center justify-between rounded-lg px-3 py-2 text-sm',
          'border transition-all duration-150 w-full',
          'focus:outline-none focus:ring-2 focus:ring-offset-0',
          'disabled:opacity-50 disabled:cursor-not-allowed',
          isGlass ? 'bg-white' : 'bg-white dark:bg-secondary-800',
          isGlass ? 'text-secondary-900' : 'text-secondary-900 dark:text-secondary-100',
          error
            ? 'border-red-500 focus:border-red-500 focus:ring-red-500/20'
            : 'border-secondary-300 dark:border-secondary-600 focus:border-primary-500 focus:ring-primary-500/20',
        ]"
      >
        <SelectValue :placeholder="placeholder" />
        <SelectIcon>
          <Icon name="lucide:chevron-down" class="w-4 h-4" />
        </SelectIcon>
      </SelectTrigger>

      <SelectPortal>
        <SelectContent
          :class="[
            'min-w-[var(--reka-select-trigger-width)] overflow-hidden rounded-lg border shadow-lg',
            'data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95',
            isGlass
              ? 'bg-white border-secondary-200'
              : 'bg-white dark:bg-secondary-800 border-secondary-200 dark:border-secondary-700',
          ]"
          position="popper"
          :side-offset="4"
        >
          <SelectViewport class="p-1">
            <SelectItem
              v-for="option in options"
              :key="option.value"
              :value="option.value"
              :disabled="option.disabled"
              :class="[
                'relative flex items-center px-3 py-2 text-sm rounded-md cursor-pointer select-none',
                'outline-none transition-colors',
                'data-[highlighted]:bg-primary-600 data-[highlighted]:text-white',
                'data-[disabled]:opacity-50 data-[disabled]:cursor-not-allowed',
                isGlass ? 'text-secondary-900' : 'text-secondary-900 dark:text-secondary-100',
              ]"
            >
              <SelectItemText>
                {{ option.label }}
              </SelectItemText>
              <SelectItemIndicator class="absolute left-2 inline-flex items-center">
                <Icon name="lucide:check" class="w-4 h-4" />
              </SelectItemIndicator>
            </SelectItem>
          </SelectViewport>
        </SelectContent>
      </SelectPortal>
    </SelectRoot>

    <p v-if="error" class="text-xs text-red-500 flex items-center gap-1">
      <Icon name="lucide:alert-circle" class="w-3.5 h-3.5" />
      {{ error }}
    </p>
  </div>
</template>
