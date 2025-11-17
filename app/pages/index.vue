<template>
  <main>
    <!-- HERO -->
    <section class="max-w-6xl mx-auto px-4 sm:px-6 py-14 sm:py-20 pt-24 sm:pt-32">
      <div class="grid items-center gap-8 sm:gap-12 md:grid-cols-[1.4fr,1fr]">
        <div>
          <p class="font-mono text-sm text-zinc-400">
            &gt; Building real‑estate platforms with .NET + Vue
          </p>
          <h1 class="mt-3 text-4xl/tight sm:text-5xl/tight font-extrabold">
            Hi, I’m <span class="text-white">Ilias El Makrini</span>
          </h1>
          <p class="mt-4 max-w-2xl text-zinc-300">
            Full‑stack engineer focused on clean architecture, integrations, and performance. I build robust, data‑driven systems connecting property, finance, and people.
          </p>

          <div class="mt-6 flex items-center gap-3">
            <a href="#projects" class="inline-flex items-center gap-2 rounded-xl bg-accent-400/90 px-4 py-2 font-medium text-white shadow-glow hover:bg-accent-500 transition">View My Work
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="h-4 w-4 fill-none stroke-current"><path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M5 12h14M12 5l7 7-7 7"
              /></svg>
            </a>
            <a href="#contact" class="inline-flex items-center gap-2 rounded-xl border border-white/10 px-4 py-2 text-zinc-200 hover:border-white/25">Contact</a>
          </div>
        </div>

        <!-- About card -->
        <div class="card">
          <div class="flex items-start gap-4">
            <div class="h-16 w-16 shrink-0 rounded-full bg-gradient-to-br from-accent-400/30 to-accent-300/20 ring-1 ring-white/10" />
            <div>
              <h3 class="font-semibold">
                About
              </h3>
              <p class="mt-1 text-sm text-zinc-300">
                I design and engineer platforms across leasing, finance, and communications. My toolkit includes .NET 9, EF Core, Azure, Vue 3, SQL Server, Rebus, and Tailwind.
              </p>
              <div class="mt-3 flex flex-wrap gap-2">
                <TechBadge label=".NET 9" />
                <TechBadge label="EF Core" />
                <TechBadge label="Azure" />
                <TechBadge label="Vue 3" />
                <TechBadge label="Rebus" />
                <TechBadge label="SQL Server" />
                <TechBadge label="TailwindCSS" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- PROJECTS -->
    <section id="projects" class="max-w-6xl mx-auto px-4 sm:px-6 py-14 sm:py-20">
      <div class="flex items-end justify-between">
        <h2 class="text-2xl sm:text-3xl font-semibold tracking-tight">
          Projects
        </h2>
        <a href="https://github.com/" target="_blank" class="text-sm text-zinc-400 hover:text-zinc-200">View GitHub →</a>
      </div>

      <div class="mt-6 grid gap-5 sm:grid-cols-2 lg:grid-cols-3">
        <ProjectCard title="ReX Platform" description="Enterprise real‑estate management suite integrating finance, leasing, and communication modules." tech=".NET · Vue · Azure" />
        <ProjectCard title="Peppol API" description="Peppol BIS UBL 3.0 invoicing pipeline with attachments, validations, and delivery tracking." tech=".NET · UBL · Azure Service Bus" />
        <ProjectCard title="JSReport Templates" description="Tailwind‑styled, multi‑language PDF templates with advanced pagination and page‑break logic." tech="Node · Handlebars · Tailwind" />
      </div>
    </section>

    <!-- EXPERIENCE / TIMELINE -->
    <section class="max-w-6xl mx-auto px-4 sm:px-6 py-14 sm:py-20">
      <h2 class="text-2xl sm:text-3xl font-semibold tracking-tight">
        Experience
      </h2>
      <ol class="mt-6 space-y-6">
        <TimelineItem
          company="Ceusters"
          role="Lead Developer"
          period="2021 → Now"
          summary="Architecting the ReX ecosystem (.NET 9 / Vue 3 / Azure SB). Designing multi‑tenant modules and invoice delivery (Peppol, email)."
        />
      </ol>
    </section>

    <!-- CONTACT -->
    <section id="contact" class="max-w-6xl mx-auto px-4 sm:px-6 py-14 sm:py-20 pb-24">
      <div class="bg-zinc-900/70 border border-white/5 rounded-2xl p-5 sm:p-6 backdrop-blur text-center">
        <h2 class="text-2xl sm:text-3xl font-semibold tracking-tight">
          Let’s build something together
        </h2>
        <p class="mt-2 text-zinc-400">
          Email, LinkedIn, and GitHub — pick your channel.
        </p>
        <div class="mt-6 flex flex-wrap items-center justify-center gap-3">
          <a href="mailto:hello@example.com" class="inline-flex items-center gap-1 rounded-full bg-white/5 border border-white/10 px-3 py-1 text-xs text-zinc-300 transition-transform duration-300 hover:-translate-y-1 hover:shadow-lg hover:border-blue-500/40">✉️ Email</a>
          <a href="https://www.linkedin.com/in/" target="_blank" class="inline-flex items-center gap-1 rounded-full bg-white/5 border border-white/10 px-3 py-1 text-xs text-zinc-300 transition-transform duration-300 hover:-translate-y-1 hover:shadow-lg hover:border-blue-500/40">💼 LinkedIn</a>
          <a href="https://github.com/" target="_blank" class="inline-flex items-center gap-1 rounded-full bg-white/5 border border-white/10 px-3 py-1 text-xs text-zinc-300 transition-transform duration-300 hover:-translate-y-1 hover:shadow-lg hover:border-blue-500/40">🐙 GitHub</a>
        </div>
      </div>
    </section>
  </main>
</template>

<script setup lang="ts">
import TimelineItem from '~/components/TimelineItem.vue'

// No page‑specific logic needed yet; keep it static for speed.
</script>
