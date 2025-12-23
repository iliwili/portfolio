<template>
  <div class="relative my-6">
    <div class="absolute inset-0 flex items-center">
      <Separator />
    </div>
    <div v-if="text" class="relative flex justify-center text-sm">
      <span class="px-2 bg-card text-muted-foreground">{{ text }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Separator } from '~/components/ui/separator'

export interface UDividerProps {
  text?: string
}

withDefaults(defineProps<UDividerProps>(), {
  text: '',
})
</script>
