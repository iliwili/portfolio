<script setup lang="ts">
import { DialogClose, DialogContent, DialogDescription, DialogOverlay, DialogPortal, DialogRoot, DialogTitle, DialogTrigger } from 'reka-ui'

export interface BaseDialogProps {
  open?: boolean
  title?: string
  description?: string
  size?: 'sm' | 'md' | 'lg' | 'xl'
}

withDefaults(defineProps<BaseDialogProps>(), {
  open: undefined,
  size: 'md',
  title: '',
  description: '',
})

const emit = defineEmits<{
  'update:open': [value: boolean]
}>()

const sizeClasses: Record<string, string> = {
  sm: 'max-w-sm',
  md: 'max-w-md',
  lg: 'max-w-lg',
  xl: 'max-w-xl',
}
</script>

<template>
  <DialogRoot :open="open" @update:open="emit('update:open', $event)">
    <DialogTrigger as-child>
      <slot name="trigger" />
    </DialogTrigger>

    <DialogPortal>
      <DialogOverlay
        class="fixed inset-0 z-50 bg-black/50 backdrop-blur-sm data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0"
      />
      <DialogContent
        :class="[
          'fixed left-[50%] top-[50%] z-50 w-full translate-x-[-50%] translate-y-[-50%] gap-4 bg-white dark:bg-secondary-800 p-6 shadow-2xl duration-200',
          'data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95 data-[state=closed]:slide-out-to-left-1/2 data-[state=closed]:slide-out-to-top-[48%] data-[state=open]:slide-in-from-left-1/2 data-[state=open]:slide-in-from-top-[48%]',
          'rounded-2xl border border-secondary-200 dark:border-secondary-700',
          sizeClasses[size],
        ]"
      >
        <div class="flex flex-col space-y-2">
          <DialogTitle
            v-if="title || $slots.title"
            class="text-xl font-semibold text-secondary-900 dark:text-white"
          >
            <slot name="title">
              {{ title }}
            </slot>
          </DialogTitle>
          <DialogDescription
            v-if="description || $slots.description"
            class="text-sm text-secondary-500 dark:text-secondary-400"
          >
            <slot name="description">
              {{ description }}
            </slot>
          </DialogDescription>
        </div>

        <div class="py-4">
          <slot />
        </div>

        <div v-if="$slots.footer" class="flex justify-end gap-2">
          <slot name="footer" />
        </div>

        <DialogClose
          class="absolute right-4 top-4 rounded-lg p-1 text-secondary-400 hover:text-secondary-600 dark:hover:text-secondary-300 hover:bg-secondary-100 dark:hover:bg-secondary-700 transition-colors"
        >
          <Icon name="lucide:x" class="w-5 h-5" />
        </DialogClose>
      </DialogContent>
    </DialogPortal>
  </DialogRoot>
</template>
