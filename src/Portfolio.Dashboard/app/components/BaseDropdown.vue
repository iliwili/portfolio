<script setup lang="ts">
import { DropdownMenuArrow, DropdownMenuContent, DropdownMenuItem, DropdownMenuPortal, DropdownMenuRoot, DropdownMenuSeparator, DropdownMenuTrigger } from 'reka-ui'

export interface BaseDropdownItem {
  label: string
  icon?: string
  onClick?: () => void
  disabled?: boolean
  separator?: boolean
}

export interface BaseDropdownProps {
  items: BaseDropdownItem[]
  align?: 'start' | 'center' | 'end'
  side?: 'top' | 'right' | 'bottom' | 'left'
}

withDefaults(defineProps<BaseDropdownProps>(), {
  align: 'end',
  side: 'bottom',
})
</script>

<template>
  <DropdownMenuRoot>
    <DropdownMenuTrigger as-child>
      <slot name="trigger" />
    </DropdownMenuTrigger>

    <DropdownMenuPortal>
      <DropdownMenuContent
        :align="align"
        :side="side"
        :side-offset="4"
        class="z-50 min-w-48 overflow-hidden rounded-lg border border-secondary-200 dark:border-secondary-700 bg-white dark:bg-secondary-800 p-1 shadow-lg data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95"
      >
        <template v-for="(item, index) in items" :key="index">
          <DropdownMenuSeparator
            v-if="item.separator"
            class="my-1 h-px bg-secondary-200 dark:bg-secondary-700"
          />
          <DropdownMenuItem
            v-else
            :disabled="item.disabled"
            class="relative flex items-center gap-2 rounded-md px-3 py-2 text-sm cursor-pointer select-none outline-none transition-colors data-highlighted:bg-secondary-100 dark:data-highlighted:bg-secondary-700 data-disabled:opacity-50 data-disabled:cursor-not-allowed text-secondary-700 dark:text-secondary-200"
            @click="item.onClick?.()"
          >
            <Icon v-if="item.icon" :name="item.icon" class="w-4 h-4" />
            <span>{{ item.label }}</span>
          </DropdownMenuItem>
        </template>

        <DropdownMenuArrow class="fill-white dark:fill-secondary-800" />
      </DropdownMenuContent>
    </DropdownMenuPortal>
  </DropdownMenuRoot>
</template>
