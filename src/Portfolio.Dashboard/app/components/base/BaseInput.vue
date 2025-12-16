<template>
  <div class="relative">
    <input
      :type="inputType"
      :value="modelValue"
      :placeholder="placeholder"
      :disabled="disabled"
      :required="required"
      :autocomplete="autocomplete"
      :class="[
        'w-full px-3 py-2 text-sm rounded-lg border transition-all duration-150 border-gray-300 dark:border-gray-600 focus:border-[#008060] focus:ring-[#008060]/20',
        isGlass ? 'bg-white' : 'bg-white dark:bg-gray-800',
        isGlass ? 'text-gray-900' : 'text-gray-900 dark:text-gray-100',
        'placeholder:text-gray-400 dark:placeholder:text-gray-500',
        'focus:outline-none focus:ring-2 focus:ring-offset-0',
        'disabled:opacity-50 disabled:cursor-not-allowed disabled:bg-gray-50 dark:disabled:bg-gray-900',
        type === 'password' ? 'pr-10' : '',
      ]"
      @input="handleInput"
    >

    <!-- Password toggle button -->
    <button
      v-if="type === 'password'"
      type="button"
      class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600 dark:hover:text-gray-300 transition-colors"
      @click="showPassword = !showPassword"
    >
      <Icon v-if="showPassword" name="lucide:eye-off" class="w-5 h-5" />
      <Icon v-else name="lucide:eye" class="w-5 h-5" />
    </button>
  </div>
</template>

<script setup lang="ts">
export interface BaseInputProps {
  modelValue: string
  type?: 'text' | 'email' | 'password' | 'number' | 'tel' | 'url'
  placeholder?: string
  disabled?: boolean
  required?: boolean
  autocomplete?: string
  variant?: 'default' | 'glass'
}

const props = withDefaults(defineProps<BaseInputProps>(), {
  type: 'text',
  placeholder: '',
  disabled: false,
  required: false,
  autocomplete: 'off',
  variant: 'default',
})

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()

const showPassword = ref(false)

const inputType = computed(() => {
  if (props.type === 'password') {
    return showPassword.value ? 'text' : 'password'
  }
  return props.type
})

const isGlass = computed(() => props.variant === 'glass')

function handleInput(event: Event) {
  const target = event.target as HTMLInputElement
  emit('update:modelValue', target.value)
}
</script>
