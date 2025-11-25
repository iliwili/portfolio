import config from '../../config.json'

export interface Project {
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

export interface Experience {
    company: string
    role: string
    period: string
    summary: string
    location?: string
    technologies?: string[]
    subExperiences?: Experience[]
}

export interface ContactLink {
    icon: string
    label: string
    href: string
    target?: string
}

export interface CTAButton {
    label: string
    href: string
    style: 'primary' | 'secondary'
}

export interface PortfolioConfig {
    personal: {
        name: string
        initials: string
        title: string
        headline: string
        headlineHighlight: string
        subtitle: string
        profileImage: string
        availability: {
            status: string
            isAvailable: boolean
        }
    }
    hero: {
        ctaButtons: CTAButton[]
    }
    techStack: string[]
    projects: Project[]
    experience: Experience[]
    contact: {
        sectionTitle: string
        sectionSubtitle: string
        links: ContactLink[]
    }
}

export const usePortfolioConfig = () => {
    return config as PortfolioConfig
}
