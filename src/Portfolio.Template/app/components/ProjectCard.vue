<template>
  <article
    class="bg-zinc-900/70 border border-white/5 rounded-2xl p-5 sm:p-6 backdrop-blur hover:shadow-lg hover:border-blue-500/40 cursor-pointer group"
    @click="openModal"
  >
    <div class="aspect-video w-full rounded-xl bg-gradient-to-br from-white/5 to-white/0 ring-1 ring-inset ring-white/10 mb-4 overflow-hidden">
      <img
        v-if="image"
        :src="image"
        :alt="`${title} preview`"
        class="w-full h-full object-cover group-hover:scale-105"
      >
    </div>
    <h3 class="font-semibold text-lg group-hover:text-accent-400">
      {{ title }}
    </h3>
    <p class="mt-1 text-sm text-zinc-300">
      {{ description }}
    </p>
    <div class="mt-3 flex items-center justify-between">
      <p class="text-xs text-zinc-400">
        {{ tech }}
      </p>
      <div class="flex items-center gap-2 text-xs text-accent-400">
        <span>View Details</span>
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="h-3 w-3"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M9 5l7 7-7 7"
          />
        </svg>
      </div>
    </div>
  </article>
</template>

<script setup lang="ts">
interface ProjectData {
  title: string
  description: string
  tech?: string
  image?: string
  demoUrl?: string
  githubUrl?: string
  details?: string
  features?: string[]
  challenges?: string
  results?: string
}

const props = defineProps<ProjectData>()

const emit = defineEmits<{
  openProject: [project: ProjectData]
}>()

function openModal() {
  emit('openProject', props)
}
</script>
