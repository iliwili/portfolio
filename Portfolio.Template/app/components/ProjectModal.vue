<template>
  <Teleport to="body">
    <Transition
      enter-active-class="duration-500 ease-out"
      enter-from-class="opacity-0 backdrop-blur-none"
      enter-to-class="opacity-100 backdrop-blur-md"
      leave-active-class="duration-300 ease-in"
      leave-from-class="opacity-100 backdrop-blur-md"
      leave-to-class="opacity-0 backdrop-blur-none"
    >
      <div
        v-if="show"
        class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/80 backdrop-blur-md"
        @click.self="$emit('close')"
      >
        <Transition
          enter-active-class="duration-500 ease-[cubic-bezier(0.34,1.56,0.64,1)]"
          enter-from-class="opacity-0 scale-75 -translate-y-8"
          enter-to-class="opacity-100 scale-100 translate-y-0"
          leave-active-class="duration-300 ease-[cubic-bezier(0.4,0,1,1)]"
          leave-from-class="opacity-100 scale-100 translate-y-0"
          leave-to-class="opacity-0 scale-90 translate-y-4"
        >
          <div
            v-if="show"
            class="relative max-w-4xl max-h-[90vh] w-full bg-zinc-900/95 border border-white/10 rounded-2xl overflow-hidden shadow-2xl transform-gpu"
          >
            <!-- Header -->
            <div
              class="flex items-start justify-between p-6 border-b border-white/10 opacity-0 translate-y-2 transition-all duration-700 delay-100"
              :class="show ? 'opacity-100 translate-y-0' : ''"
            >
              <div>
                <h2 class="text-2xl font-bold">
                  {{ project.title }}
                </h2>
                <p class="mt-1 text-zinc-400">
                  {{ project.tech }}
                </p>
              </div>
              <button
                class="p-2 rounded-lg hover:bg-white/10 transition-all duration-200 hover:scale-110 hover:rotate-90"
                @click="$emit('close')"
              >
                <svg
                  class="w-5 h-5"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M6 18L18 6M6 6l12 12"
                  />
                </svg>
              </button>
            </div>

            <!-- Content -->
            <div class="overflow-y-auto max-h-[calc(90vh-120px)]">
              <!-- Demo Section -->
              <div
                v-if="project.demoUrl"
                class="p-6 border-b border-white/10 opacity-0 translate-y-4 transition-all duration-700 delay-200"
                :class="show ? 'opacity-100 translate-y-0' : ''"
              >
                <h3 class="text-lg font-semibold mb-4">
                  Live Demo
                </h3>
                <div class="aspect-video rounded-xl overflow-hidden border border-white/10">
                  <iframe
                    :src="project.demoUrl"
                    class="w-full h-full opacity-0 transition-opacity duration-1000 delay-700"
                    :class="show ? 'opacity-100' : ''"
                    frameborder="0"
                    allowfullscreen
                    title="Project Demo"
                  />
                </div>
              </div>

              <!-- Project Image -->
              <div
                v-else-if="project.image"
                class="p-6 border-b border-white/10 opacity-0 translate-y-4 transition-all duration-700 delay-200"
                :class="show ? 'opacity-100 translate-y-0' : ''"
              >
                <div class="aspect-video rounded-xl overflow-hidden border border-white/10">
                  <img
                    :src="project.image"
                    :alt="`${project.title} preview`"
                    class="w-full h-full object-cover opacity-0 transition-all duration-1000 delay-700 scale-105"
                    :class="show ? 'opacity-100 scale-100' : ''"
                  >
                </div>
              </div>

              <!-- Description -->
              <div
                class="p-6 border-b border-white/10 opacity-0 translate-y-4 transition-all duration-700 delay-300"
                :class="show ? 'opacity-100 translate-y-0' : ''"
              >
                <h3 class="text-lg font-semibold mb-3">
                  About This Project
                </h3>
                <p class="text-zinc-300 leading-relaxed">
                  {{ project.details || project.description }}
                </p>
              </div>

              <!-- Features -->
              <div
                v-if="project.features?.length"
                class="p-6 border-b border-white/10 opacity-0 translate-y-4 transition-all duration-700 delay-400"
                :class="show ? 'opacity-100 translate-y-0' : ''"
              >
                <h3 class="text-lg font-semibold mb-3">
                  Key Features
                </h3>
                <ul class="space-y-2">
                  <li
                    v-for="(feature, index) in project.features"
                    :key="feature"
                    class="flex items-start gap-2 text-zinc-300 opacity-0 translate-x-2 transition-all duration-500"
                    :class="show ? 'opacity-100 translate-x-0' : ''"
                    :style="{ transitionDelay: `${600 + index * 100}ms` }"
                  >
                    <svg
                      class="w-5 h-5 text-accent-400 mt-0.5 shrink-0"
                      fill="currentColor"
                      viewBox="0 0 20 20"
                    >
                      <path
                        fill-rule="evenodd"
                        d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                        clip-rule="evenodd"
                      />
                    </svg>
                    {{ feature }}
                  </li>
                </ul>
              </div>

              <!-- Challenges & Results -->
              <div
                v-if="project.challenges || project.results"
                class="p-6 opacity-0 translate-y-4 transition-all duration-700 delay-500"
                :class="show ? 'opacity-100 translate-y-0' : ''"
              >
                <div v-if="project.challenges" class="mb-6">
                  <h3 class="text-lg font-semibold mb-3">
                    Technical Challenges
                  </h3>
                  <p class="text-zinc-300 leading-relaxed">
                    {{ project.challenges }}
                  </p>
                </div>
                <div v-if="project.results">
                  <h3 class="text-lg font-semibold mb-3">
                    Results & Impact
                  </h3>
                  <p class="text-zinc-300 leading-relaxed">
                    {{ project.results }}
                  </p>
                </div>
              </div>
            </div>

            <!-- Footer -->
            <div
              class="flex items-center justify-between p-6 border-t border-white/10 bg-zinc-950/50 opacity-0 translate-y-4 transition-all duration-700 delay-600"
              :class="show ? 'opacity-100 translate-y-0' : ''"
            >
              <div class="flex items-center gap-3">
                <NuxtLink
                  v-if="project.slug"
                  :to="`/projects/${project.slug}`"
                  class="inline-flex items-center gap-2 px-4 py-2 bg-accent-400/90 rounded-lg text-white font-medium hover:bg-accent-500 transition-colors"
                >
                  Full Case Study
                  <svg
                    class="w-4 h-4"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M10 6H6a2 2 0 00-2 2v10a2 2 0 002 2h10a2 2 0 002-2v-4M14 4h6m0 0v6m0-6L10 14"
                    />
                  </svg>
                </NuxtLink>
              </div>

              <div class="flex items-center gap-3">
                <a
                  v-if="project.githubUrl"
                  :href="project.githubUrl"
                  target="_blank"
                  class="inline-flex items-center gap-2 px-3 py-2 border border-white/20 rounded-lg text-zinc-300 hover:text-white hover:border-white/40 transition-colors"
                >
                  <svg
                    class="w-4 h-4"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path d="M12 0c-6.626 0-12 5.373-12 12 0 5.302 3.438 9.8 8.207 11.387.599.111.793-.261.793-.577v-2.234c-3.338.726-4.033-1.416-4.033-1.416-.546-1.387-1.333-1.756-1.333-1.756-1.089-.745.083-.729.083-.729 1.205.084 1.839 1.237 1.839 1.237 1.07 1.834 2.807 1.304 3.492.997.107-.775.418-1.305.762-1.604-2.665-.305-5.467-1.334-5.467-5.931 0-1.311.469-2.381 1.236-3.221-.124-.303-.535-1.524.117-3.176 0 0 1.008-.322 3.301 1.23.957-.266 1.983-.399 3.003-.404 1.02.005 2.047.138 3.006.404 2.291-1.552 3.297-1.23 3.297-1.23.653 1.653.242 2.874.118 3.176.77.84 1.235 1.911 1.235 3.221 0 4.609-2.807 5.624-5.479 5.921.43.372.823 1.102.823 2.222v3.293c0 .319.192.694.801.576 4.765-1.589 8.199-6.086 8.199-11.386 0-6.627-5.373-12-12-12z" />
                  </svg>
                  Code
                </a>
                <a
                  v-if="project.demoUrl"
                  :href="project.demoUrl"
                  target="_blank"
                  class="inline-flex items-center gap-2 px-3 py-2 border border-white/20 rounded-lg text-zinc-300 hover:text-white hover:border-white/40 transition-colors"
                >
                  Live Demo
                  <svg
                    class="w-4 h-4"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M10 6H6a2 2 0 00-2 2v10a2 2 0 002 2h10a2 2 0 002-2v-4M14 4h6m0 0v6m0-6L10 14"
                    />
                  </svg>
                </a>
              </div>
            </div>
          </div>
        </Transition>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup lang="ts">
interface Project {
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
  slug?: string
}

const props = defineProps<{
  show: boolean
  project: Project
}>()

const emit = defineEmits<{
  close: []
}>()

// Close modal on Escape key
onMounted(() => {
  const handleEscape = (e: KeyboardEvent) => {
    if (e.key === 'Escape') {
      emit('close')
    }
  }
  document.addEventListener('keydown', handleEscape)
  onUnmounted(() => {
    document.removeEventListener('keydown', handleEscape)
  })
})

// Prevent body scroll when modal is open
watch(() => props.show, (isOpen) => {
  if (isOpen) {
    document.body.style.overflow = 'hidden'
  }
  else {
    document.body.style.overflow = ''
  }
})
</script>
