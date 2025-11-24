<template>
  <main>
    <!-- HERO -->
    <section class="relative min-h-screen flex items-center">
      <!-- Background Elements -->
      <div class="absolute inset-0 overflow-hidden">
        <div class="absolute top-1/4 left-1/4 w-72 h-72 bg-accent-400/5 rounded-full blur-3xl opacity-0 transition-opacity duration-2000" :class="isLoaded ? 'opacity-100' : ''" />
        <div class="absolute bottom-1/4 right-1/4 w-96 h-96 bg-blue-400/3 rounded-full blur-3xl opacity-0 transition-opacity duration-2000 delay-500" :class="isLoaded ? 'opacity-100' : ''" />
      </div>

      <div class="relative max-w-7xl mx-auto px-4 sm:px-6 py-20">
        <!-- Split Layout Container -->
        <div class="grid lg:grid-cols-2 gap-12 lg:gap-16 items-center min-h-[70vh]">
          <!-- Left Content -->
          <div class="lg:order-1 order-2">
            <!-- Status Badge -->
            <div v-if="personal.availability.isAvailable" class="inline-flex items-center gap-3 px-4 py-2 bg-accent-400/10 border border-accent-400/20 rounded-full text-accent-400 text-sm font-medium mb-6 opacity-0 translate-y-4 transition-all duration-800 delay-100" :class="isLoaded ? 'opacity-100 translate-y-0' : ''">
              <div class="w-2 h-2 bg-green-400 rounded-full animate-pulse" />
              <span>{{ personal.availability.status }}</span>
            </div>

            <!-- Main Heading -->
            <h1 class="text-4xl sm:text-5xl lg:text-6xl font-black tracking-tight mb-6 opacity-0 translate-y-12 transition-all duration-1000 delay-200" :class="isLoaded ? 'opacity-100 translate-y-0' : ''">
              <span class="text-white">{{ personal.headline }}</span>
              <span class="block bg-gradient-to-r from-accent-400 via-blue-400 to-purple-400 bg-clip-text text-transparent">
                {{ personal.headlineHighlight }}
              </span>
            </h1>

            <!-- Subtitle -->
            <p class="text-lg sm:text-xl text-zinc-300 mb-8 leading-relaxed opacity-0 translate-y-8 transition-all duration-800 delay-400" :class="isLoaded ? 'opacity-100 translate-y-0' : ''" v-html="personal.subtitle" />

            <!-- CTA Buttons -->
            <div class="flex flex-col sm:flex-row items-start gap-4 mb-8 opacity-0 translate-y-8 transition-all duration-800 delay-600" :class="isLoaded ? 'opacity-100 translate-y-0' : ''">
              <a
                v-for="(button, index) in hero.ctaButtons"
                :key="button.label"
                :class="[
                  'inline-flex items-center gap-3 px-8 py-4 font-semibold rounded-2xl transition-all duration-200',
                  button.style === 'primary'
                    ? 'bg-gradient-to-r from-accent-400 to-blue-500 text-white shadow-lg hover:shadow-accent-400/25 hover:scale-105'
                    : 'bg-zinc-900/50 border border-white/10 text-zinc-200 hover:border-white/25 hover:bg-zinc-800/50',
                ]"
                :href="button.href"
                @click.prevent="scrollToSection(button.href.replace('#', ''))"
              >
                {{ button.label }}
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    :d="index === 0 ? 'M17 8l4 4m0 0l-4 4m4-4H3' : 'M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z'"
                  />
                </svg>
              </a>
            </div>

            <!-- Tech Stack Showcase -->
            <div class="opacity-0 translate-y-8 transition-all duration-800 delay-800" :class="isLoaded ? 'opacity-100 translate-y-0' : ''">
              <p class="text-sm text-zinc-400 mb-4 font-mono">
                &gt; Tech Stack
              </p>
              <div class="flex flex-wrap gap-2">
                <TechBadge
                  v-for="(tech, index) in techStack.slice(0, 6)"
                  :key="tech"
                  :label="tech"
                  class="opacity-0 translate-y-4 scale-75 transition-all duration-600 hover:scale-105"
                  :class="isLoaded ? 'opacity-100 translate-y-0 scale-100' : ''"
                  :style="{ transitionDelay: `${1000 + index * 100}ms` }"
                />
              </div>
            </div>
          </div>

          <!-- Right Content - Profile Image -->
          <div class="lg:order-2 order-1 flex justify-center lg:justify-end">
            <div class="relative opacity-0 translate-y-8 transition-all duration-1000 delay-300" :class="isLoaded ? 'opacity-100 translate-y-0' : ''">
              <!-- Main Image Container -->
              <div class="relative w-80 h-80 lg:w-96 lg:h-96">
                <!-- Gradient Background -->
                <div class="absolute inset-0 bg-gradient-to-br from-accent-400/20 via-blue-400/15 to-purple-400/10 rounded-3xl rotate-6 transform transition-transform duration-500 hover:rotate-3" />

                <!-- Image Frame -->
                <div class="relative w-full h-full bg-gradient-to-br from-zinc-800 to-zinc-900 rounded-3xl overflow-hidden border border-white/10 shadow-2xl shadow-accent-400/10">
                  <img
                    :src="`~/${personal.profileImage}`"
                    :alt="personal.name"
                    class="w-full h-full object-cover transition-transform duration-500 hover:scale-105"
                  >

                  <!-- Overlay with subtle pattern -->
                  <div class="absolute inset-0 bg-gradient-to-t from-zinc-900/20 via-transparent to-transparent" />
                </div>

                <!-- Floating Elements -->
                <div class="absolute -top-4 -left-4 w-12 h-12 bg-accent-400/20 rounded-2xl backdrop-blur-sm border border-accent-400/30 rotate-12 opacity-0 transition-all duration-1000 delay-700" :class="isLoaded ? 'opacity-100' : ''" />
                <div class="absolute -bottom-6 -right-6 w-16 h-16 bg-blue-400/20 rounded-2xl backdrop-blur-sm border border-blue-400/30 -rotate-12 opacity-0 transition-all duration-1000 delay-900" :class="isLoaded ? 'opacity-100' : ''" />
              </div>
            </div>
          </div>
        </div>

        <!-- Scroll Indicator -->
        <div
          class="absolute bottom-8 left-1/2 -translate-x-1/2 opacity-0 translate-y-4 transition-all duration-800"
          :class="{
            'opacity-100 translate-y-0': isLoaded && showScrollIndicator,
            'opacity-0 translate-y-4': !isLoaded || !showScrollIndicator,
          }"
          :style="{ transitionDelay: (showScrollIndicator && !hasScrollIndicatorAppeared) ? '2000ms' : '0ms' }"
        >
          <div class="flex flex-col items-center gap-2 text-zinc-400 hover:text-white transition-colors">
            <span class="text-xs font-medium">Scroll to explore</span>
            <div class="w-6 h-10 border-2 border-current rounded-full flex justify-center">
              <div class="w-1 h-3 bg-current rounded-full mt-2 animate-bounce" />
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- PROJECTS -->
    <section id="projects" class="max-w-7xl mx-auto px-4 sm:px-6 py-6 sm:py-20">
      <div class="flex items-end justify-between opacity-0 translate-y-8 scale-95 transition-all duration-1000 ease-out" :class="projectsVisible ? 'opacity-100 translate-y-0 scale-100' : ''">
        <div>
          <h2 class="text-2xl sm:text-3xl font-semibold tracking-tight">
            Projects
          </h2>
          <p class="text-sm text-zinc-400 mt-2">
            Scroll to explore my work
          </p>
        </div>
        <div class="flex items-center gap-3">
          <div class="flex gap-2">
            <button
              class="p-2 rounded-full bg-zinc-800/50 border border-white/10 text-zinc-400 hover:text-white hover:border-white/25 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed"
              :disabled="!canScrollLeft"
              @click="scrollProjects('left')"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-4 w-4"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M15 19l-7-7 7-7"
                />
              </svg>
            </button>
            <button
              class="p-2 rounded-full bg-zinc-800/50 border border-white/10 text-zinc-400 hover:text-white hover:border-white/25 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed"
              :disabled="!canScrollRight"
              @click="scrollProjects('right')"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-4 w-4"
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
            </button>
          </div>
          <a href="https://github.com/" target="_blank" class="text-sm text-zinc-400 hover:text-zinc-200">View GitHub →</a>
        </div>
      </div>

      <!-- Horizontal Scrolling Projects -->
      <div class="mt-8 opacity-0 translate-y-8 scale-95 transition-all duration-1000 delay-200 ease-out" :class="projectsVisible ? 'opacity-100 translate-y-0 scale-100' : ''">
        <div
          ref="projectsContainer"
          class="flex gap-5 overflow-x-auto scroll-smooth pb-4 scrollbar-hide"
          @scroll="updateScrollButtons"
        >
          <ProjectCard
            v-for="(project, index) in projects"
            :key="project.title"
            :title="project.title"
            :description="project.description"
            :tech="project.tech"
            :image="project.image"
            :demo-url="project.demoUrl"
            :github-url="project.githubUrl"
            :details="project.details"
            :features="project.features"
            :challenges="project.challenges"
            :results="project.results"
            :slug="project.slug"
            class="flex-none w-80 sm:w-96 opacity-0 scale-90 transition-all duration-700 ease-out"
            :class="projectsVisible ? 'opacity-100 scale-100' : ''"
            :style="{ transitionDelay: `${400 + index * 150}ms` }"
            @open-project="openProjectModal"
          />
        </div>
      </div>

      <!-- Project Modal -->
      <ProjectModal
        :show="showModal"
        :project="selectedProject"
        @close="closeModal"
      />
    </section>

    <!-- EXPERIENCE / TIMELINE -->
    <section id="timeline" class="max-w-7xl mx-auto px-4 sm:px-6 py-14 sm:py-20">
      <div class="flex items-end justify-between opacity-0 translate-y-8 scale-90 transition-all duration-1000 ease-out" :class="timelineVisible ? 'opacity-100 translate-y-0 scale-100' : ''">
        <div>
          <h2 class="text-2xl sm:text-3xl font-semibold tracking-tight">
            Experience
          </h2>
          <p class="text-sm text-zinc-400 mt-2">
            My professional journey and technical expertise
          </p>
        </div>
      </div>
      <ol class="mt-12 space-y-6 px-6">
        <div v-for="(experience, index) in experiences" :key="`${experience.company}-${experience.period}`">
          <!-- Main Experience -->
          <TimelineItem
            :company="experience.company"
            :role="experience.role"
            :period="experience.period"
            :summary="experience.summary"
            :location="experience.location"
            :technologies="experience.technologies"
            class="opacity-0 translate-y-6 scale-98 transition-all duration-900 ease-out"
            :class="timelineVisible ? 'opacity-100 translate-y-0 scale-100' : ''"
            :style="{ transitionDelay: `${index * 150}ms` }"
          />

          <!-- Sub-experiences (client assignments) -->
          <div v-if="experience.subExperiences" class="flex flex-col gap-4 mt-12">
            <TimelineItem
              v-for="(subExp, subIndex) in experience.subExperiences"
              :key="`${subExp.company}-${subExp.period}`"
              :company="subExp.company"
              :role="subExp.role"
              :period="subExp.period"
              :summary="subExp.summary"
              :location="subExp.location"
              :technologies="subExp.technologies"
              :is-sub-experience="true"
              class="opacity-0 translate-x-4 scale-95 transition-all duration-700 ease-out"
              :class="timelineVisible ? 'opacity-100 translate-x-0 scale-100' : ''"
              :style="{ transitionDelay: `${(index * 150) + (subIndex * 100) + 200}ms` }"
            />
          </div>
        </div>
      </ol>
    </section>

    <!-- CONTACT -->
    <section id="contact" class="max-w-6xl mx-auto px-4 sm:px-6 py-14 sm:py-20 pb-24">
      <div class="bg-zinc-900/70 border border-white/5 rounded-2xl p-5 sm:p-6 backdrop-blur text-center opacity-0 translate-y-12 scale-90 transition-all duration-1000 ease-out" :class="contactVisible ? 'opacity-100 translate-y-0 scale-100' : ''">
        <h2 class="text-2xl sm:text-3xl font-semibold tracking-tight">
          {{ contact.sectionTitle }}
        </h2>
        <p class="mt-2 text-zinc-400">
          {{ contact.sectionSubtitle }}
        </p>
        <div class="mt-6 flex flex-wrap items-center justify-center gap-3">
          <a
            v-for="(contactLink, index) in contactLinks"
            :key="contactLink.label"
            :href="contactLink.href"
            :target="contactLink.target"
            class="inline-flex items-center gap-1 rounded-full bg-white/5 border border-white/10 px-3 py-1 text-xs text-zinc-300 transition-all duration-600 hover:-translate-y-1 hover:shadow-lg hover:border-blue-500/40 opacity-0 translate-y-6 scale-75"
            :class="contactVisible ? 'opacity-100 translate-y-0 scale-100' : ''"
            :style="{ transitionDelay: `${300 + index * 100}ms` }"
          >
            {{ contactLink.icon }} {{ contactLink.label }}
          </a>
        </div>
      </div>
    </section>
  </main>
</template>

<script setup lang="ts">
import TimelineItem from '~/components/TimelineItem.vue'
import ProjectCard from '~/components/ProjectCard.vue'
import ProjectModal from '~/components/ProjectModal.vue'
import type { Project } from '~/composables/usePortfolioConfig'

const config = usePortfolioConfig()

// Extract data from config
const { personal, hero, techStack, projects, experience: experiences, contact } = config
const contactLinks = contact.links

// Animation state
const isLoaded = ref(false)
const showScrollIndicator = ref(true)
const hasScrollIndicatorAppeared = ref(false)

// Scroll-triggered animation state
const projectsVisible = ref(false)
const timelineVisible = ref(false)
const contactVisible = ref(false)

// Horizontal scroll controls
const projectsContainer = ref<HTMLElement | null>(null)
const canScrollLeft = ref(false)
const canScrollRight = ref(true)

const showModal = ref(false)
const selectedProject = ref<Project>({
  title: '',
  description: '',
  tech: '',
})

function openProjectModal(project: Project) {
  selectedProject.value = project
  showModal.value = true
}

function closeModal() {
  showModal.value = false
}

function scrollToSection(sectionId: string) {
  const element = document.getElementById(sectionId)
  if (element) {
    element.scrollIntoView({
      behavior: 'smooth',
      block: 'start',
    })
  }
}

function scrollProjects(direction: 'left' | 'right') {
  if (!projectsContainer.value) return

  const scrollAmount = 400
  const currentScroll = projectsContainer.value.scrollLeft
  const targetScroll = direction === 'left'
    ? currentScroll - scrollAmount
    : currentScroll + scrollAmount

  projectsContainer.value.scrollTo({
    left: targetScroll,
    behavior: 'smooth',
  })
}

function updateScrollButtons() {
  if (!projectsContainer.value) return

  const container = projectsContainer.value
  canScrollLeft.value = container.scrollLeft > 0
  canScrollRight.value
    = container.scrollLeft < container.scrollWidth - container.clientWidth - 10
}

// Trigger animations on page load
onMounted(() => {
  // Trigger hero animations immediately
  console.log('🎬 Starting page animations...')
  isLoaded.value = true

  // Setup Intersection Observer for scroll-triggered animations
  const observerOptions = {
    root: null,
    rootMargin: '0px',
    threshold: 0.1, // Trigger when 10% of element is visible
  }

  const observer = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        const target = entry.target as HTMLElement
        if (target.id === 'projects') {
          projectsVisible.value = true
        }
        else if (target.id === 'timeline') {
          timelineVisible.value = true
        }
        else if (target.id === 'contact') {
          contactVisible.value = true
        }
      }
    })
  }, observerOptions)

  // Observe sections
  nextTick(() => {
    const projectsSection = document.getElementById('projects')
    const timelineSection = document.getElementById('timeline')
    const contactSection = document.getElementById('contact')

    if (projectsSection) observer.observe(projectsSection)
    if (timelineSection) observer.observe(timelineSection)
    if (contactSection) observer.observe(contactSection)
  })

  // Add scroll listener to hide scroll indicator
  const handleScroll = () => {
    const scrollY = window.scrollY
    const shouldShow = scrollY < 50

    if (showScrollIndicator.value && !shouldShow) {
      // Mark that indicator has appeared when it first disappears
      hasScrollIndicatorAppeared.value = true
    }

    showScrollIndicator.value = shouldShow
  }

  window.addEventListener('scroll', handleScroll)

  // Update scroll buttons when projects container is ready
  nextTick(() => {
    updateScrollButtons()
  })

  // Cleanup on unmount
  onUnmounted(() => {
    window.removeEventListener('scroll', handleScroll)
    observer.disconnect()
  })
})
</script>

<style>
.scrollbar-hide {
  -ms-overflow-style: none;
  scrollbar-width: none;
}

.scrollbar-hide::-webkit-scrollbar {
  display: none;
}
</style>
