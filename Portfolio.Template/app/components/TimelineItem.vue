<template>
  <li class="relative" :class="isSubExperience ? 'ml-6' : ''">
    <!-- Timeline line (vertical connector) -->
    <div
      v-if="!isSubExperience"
      class="absolute left-1.5 top-6 bottom-0 w-0.5 bg-zinc-800"
    />

    <!-- Main timeline dot -->
    <div
      v-if="!isSubExperience"
      class="absolute left-0 top-2 w-4 h-4 bg-accent-400 rounded-full ring-4 ring-zinc-900 border-2 border-accent-500"
    />

    <!-- Sub-experience connector and dot -->
    <template v-if="isSubExperience">
      <!-- Connecting line to main timeline -->
      <div class="absolute -left-0 top-7 bottom-0 w-0.5 bg-zinc-700" />
      <!-- Sub-experience dot -->
      <div class="absolute -left-2 top-2 w-4.5 h-4.5 bg-zinc-500 rounded-full border-2 border-zinc-800" />
    </template>

    <!-- Content card -->
    <div
      :class="[
        isSubExperience ? 'ml-6' : 'ml-8',
        isSubExperience ? 'p-4 bg-zinc-900/40 border border-zinc-800/60 rounded-lg' : 'card',
      ]"
    >
      <div class="flex flex-wrap items-start justify-between gap-2">
        <div class="flex-1">
          <h3
            class="font-semibold"
            :class="isSubExperience ? 'text-zinc-200 text-sm' : 'text-white text-lg'"
          >
            {{ company }}
          </h3>
          <p
            class="mt-1 text-zinc-300"
            :class="isSubExperience ? 'text-xs' : 'text-sm'"
          >
            {{ role }}
          </p>
          <div v-if="location" class="mt-1 flex items-center gap-1 text-xs text-zinc-400">
            <span class="text-zinc-500">📍</span>
            <span>{{ location }}</span>
          </div>
        </div>
        <span
          class="text-zinc-400 font-mono shrink-0"
          :class="isSubExperience ? 'text-xs' : 'text-sm'"
        >
          {{ period }}
        </span>
      </div>

      <p
        class="mt-3 text-zinc-300 leading-relaxed"
        :class="isSubExperience ? 'text-xs' : 'text-sm'"
      >
        {{ summary }}
      </p>

      <div v-if="technologies && technologies.length" class="mt-3 flex flex-wrap gap-1.5">
        <TechBadge
          v-for="tech in technologies"
          :key="tech"
          :label="tech"
          :class="isSubExperience ? 'scale-90' : ''"
        />
      </div>
    </div>
  </li>
</template>

<script setup lang="ts">
import TechBadge from '~/components/TechBadge.vue'

interface TimelineProps {
  company: string
  role: string
  period: string
  summary: string
  location?: string
  technologies?: string[]
  isSubExperience?: boolean
}

defineProps<TimelineProps>()
</script>
