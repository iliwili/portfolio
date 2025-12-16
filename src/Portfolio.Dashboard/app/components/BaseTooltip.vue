<script setup lang="ts">
import { TooltipArrow, TooltipContent, TooltipPortal, TooltipProvider, TooltipRoot, TooltipTrigger } from 'reka-ui'

export interface BaseTooltipProps {
  content?: string
  side?: 'top' | 'right' | 'bottom' | 'left'
  align?: 'start' | 'center' | 'end'
  delayDuration?: number
}

withDefaults(defineProps<BaseTooltipProps>(), {
  content: '',
  side: 'top',
  align: 'center',
  delayDuration: 300,
})
</script>

<template>
  <TooltipProvider>
    <TooltipRoot :delay-duration="delayDuration">
      <TooltipTrigger as-child>
        <slot />
      </TooltipTrigger>

      <TooltipPortal>
        <TooltipContent
          :side="side"
          :align="align"
          :side-offset="4"
          class="z-50 overflow-hidden rounded-lg bg-gray-900 dark:bg-gray-700 px-3 py-1.5 text-xs text-white shadow-lg animate-in fade-in-0 zoom-in-95 data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=closed]:zoom-out-95"
        >
          <slot name="content">
            {{ content }}
          </slot>
          <TooltipArrow class="fill-gray-900 dark:fill-gray-700" />
        </TooltipContent>
      </TooltipPortal>
    </TooltipRoot>
  </TooltipProvider>
</template>
