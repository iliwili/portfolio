<template>
  <div class="w-full max-w-md">
    <BaseCard variant="glass">
      <!-- Header -->
      <div class="text-center mb-6">
        <h1 class="text-xl font-semibold text-gray-900">
          Create an account
        </h1>
        <p class="text-gray-500 mt-1 text-sm">
          Start building your portfolio today
        </p>
      </div>

      <div class="space-y-4">
        <div class="grid grid-cols-2 gap-3">
          <FormField
            name="firstName"
            label="First name"
            required
            :errors="errors"
            variant="glass"
          >
            <BaseInput
              v-model="request.firstName"
              type="text"
              placeholder="John"
              autocomplete="given-name"
              variant="glass"
              required
            />
          </FormField>

          <FormField
            name="lastName"
            label="Last name"
            required
            :errors="errors"
            variant="glass"
          >
            <BaseInput
              v-model="request.lastName"
              type="text"
              placeholder="Doe"
              autocomplete="family-name"
              variant="glass"
              required
            />
          </FormField>
        </div>

        <FormField
          name="userName"
          label="Username"
          required
          :errors="errors"
          variant="glass"
        >
          <BaseInput
            v-model="request.userName"
            type="text"
            placeholder="johndoe"
            autocomplete="username"
            variant="glass"
            required
          />
        </FormField>

        <FormField
          name="email"
          label="Email"
          required
          :errors="errors"
          variant="glass"
        >
          <BaseInput
            v-model="request.email"
            type="email"
            placeholder="you@example.com"
            autocomplete="email"
            variant="glass"
            required
          />
        </FormField>

        <FormField
          name="accountName"
          label="Site name"
          required
          :errors="errors"
          :hint="siteNameHint"
          variant="glass"
        >
          <BaseInput
            v-model="request.accountName"
            type="text"
            placeholder="My Portfolio"
            variant="glass"
            required
          />
        </FormField>

        <FormField
          name="password"
          label="Password"
          required
          :errors="errors"
          variant="glass"
        >
          <BaseInput
            v-model="request.password"
            type="password"
            placeholder="Create a password"
            autocomplete="new-password"
            variant="glass"
            required
          />
        </FormField>

        <FormField
          name="confirmPassword"
          label="Confirm password"
          required
          :errors="errors"
          variant="glass"
        >
          <BaseInput
            v-model="request.confirmPassword"
            type="password"
            placeholder="Confirm your password"
            autocomplete="new-password"
            variant="glass"
            required
          />
        </FormField>

        <BaseButton
          type="submit"
          variant="primary"
          size="lg"
          full-width
          @click="submit"
        >
          Create account
        </BaseButton>
      </div>

      <BaseDivider text="or" variant="glass" />

      <BaseButton variant="outline-glass" full-width>
        <Icon name="logos:github-icon" class="w-4 h-4 mr-2" />
        Continue with GitHub
      </BaseButton>

      <p class="text-center text-sm text-gray-500 mt-6">
        Already have an account?
        <BaseLink to="/auth/login" variant="glass">
          Log in
        </BaseLink>
      </p>
    </BaseCard>
  </div>
</template>

<script setup lang="ts">
import { registerSchema, type RegisterFormData } from './models/register'
import type { RegisterRequest } from '~/api/portfolio.api.generated.clients'
import { useValidate } from '~/composables/useValidate'
import FormField from '~/components/FormField.vue'

definePageMeta({
  layout: 'auth',
})

const auth = useAuth()
const request = ref<RegisterFormData>({
  firstName: '',
  lastName: '',
  userName: '',
  email: '',
  accountName: '',
  slug: '',
  password: '',
  confirmPassword: '',
})

const { validate, errors, addFromProblem } = useValidate(registerSchema)
const { clearFormError, applyError } = useProblemFormErrors(addFromProblem)

const siteNameHint = computed(() => {
  if (!request.value.accountName)
    return 'Your site will be available at {siteName}.portiva.com'
  const slug = request.value.accountName.toLowerCase().replace(/\s+/g, '-').replace(/[^a-z0-9-]/g, '')
  request.value.slug = slug
  return `${slug}.portiva.com`
})

async function submit() {
  clearFormError()
  if (!validate(request.value)) return

  try {
    const requestBody = {
      firstName: request.value.firstName,
      lastName: request.value.lastName,
      userName: request.value.userName,
      email: request.value.email,
      password: request.value.password,
      accountName: request.value.accountName,
      slug: request.value.slug,
    } as RegisterRequest

    await auth.register(requestBody)
    await navigateTo('/auth/verify-email')
  }
  catch (e) {
    applyError(e)
  }
}
</script>
