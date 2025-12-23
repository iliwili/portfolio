<script setup lang="ts">
import type { HTMLAttributes } from 'vue'
import { EyeIcon, EyeOffIcon } from 'lucide-vue-next'
import { useVModel } from '@vueuse/core'
import { cn } from '@/lib/utils'
import { Button } from '../button'

const props = defineProps<{
  defaultValue?: string
  modelValue?: string
  class?: HTMLAttributes['class']
  placeholder?: string
  autocomplete?: string
  required?: boolean
  disabled?: boolean
}>()

const emits = defineEmits<{
  (e: 'update:modelValue', payload: string): void
}>()

const modelValue = useVModel(props, 'modelValue', emits, {
  passive: true,
  defaultValue: props.defaultValue,
})

const showPassword = ref(false)

const togglePassword = () => {
  showPassword.value = !showPassword.value
}
</script>

<template>
  <div class="relative">
    <input
      v-model="modelValue"
      data-slot="input"
      :type="showPassword ? 'text' : 'password'"
      :placeholder="placeholder"
      :autocomplete="autocomplete"
      :required="required"
      :disabled="disabled"
      :class="cn(
        'file:text-foreground placeholder:text-muted-foreground selection:bg-primary selection:text-primary-foreground dark:bg-input/30 border-input h-9 w-full min-w-0 rounded-md border bg-transparent px-3 py-1 text-base shadow-xs transition-[color,box-shadow] outline-none file:inline-flex file:h-7 file:border-0 file:bg-transparent file:text-sm file:font-medium disabled:pointer-events-none disabled:cursor-not-allowed disabled:opacity-50 md:text-sm pr-10',
        'focus-visible:border-ring focus-visible:ring-ring/50 focus-visible:ring-[3px]',
        'aria-invalid:ring-destructive/20 dark:aria-invalid:ring-destructive/40 aria-invalid:border-destructive',
        props.class,
      )"
    >
    <Button
      type="button"
      variant="ghost"
      size="icon-sm"
      class="absolute right-1 top-1/2 -translate-y-1/2 h-7 w-7"
      :aria-label="showPassword ? 'Hide password' : 'Show password'"
      @click="togglePassword"
    >
      <EyeOffIcon v-if="showPassword" class="h-4 w-4 text-muted-foreground" />
      <EyeIcon v-else class="h-4 w-4 text-muted-foreground" />
    </Button>
  </div>
</template>
