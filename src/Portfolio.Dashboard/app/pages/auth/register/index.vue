<template>
  <div class="w-full max-w-md">
    <UCard>
      <div class="p-8">
        <!-- Header -->
        <div class="mb-8">
          <h1 class="text-2xl font-semibold text-foreground">
            Create an account
          </h1>
          <p class="text-muted-foreground mt-1">
            Start building your portfolio today
          </p>
        </div>

        <div class="space-y-4">
          <div class="grid grid-cols-2 gap-3">
            <UFormField
              name="firstName"
              label="First name"
              :required="true"
              :errors="errors"
            >
              <Input
                v-model="request.firstName"
                type="text"
                placeholder="John"
                autocomplete="given-name"
                required
              />
            </UFormField>

            <UFormField
              name="lastName"
              label="Last name"
              :required="true"
              :errors="errors"
            >
              <Input
                v-model="request.lastName"
                type="text"
                placeholder="Doe"
                autocomplete="family-name"
                required
              />
            </UFormField>
          </div>

          <UFormField
            name="userName"
            label="Username"
            :required="true"
            :error="errors"
          >
            <Input
              v-model="request.userName"
              type="text"
              placeholder="johndoe"
              autocomplete="username"
              required
            />
          </UFormField>

          <UFormField
            name="email"
            label="Email"
            :required="true"
            :errors="errors"
          >
            <Input
              v-model="request.email"
              type="email"
              placeholder="you@example.com"
              autocomplete="email"
              required
            />
          </UFormField>

          <UFormField
            name="accountName"
            label="Site name"
            :required="true"
            :errors="errors"
            :hint="siteNameHint"
          >
            <Input
              v-model="request.accountName"
              type="text"
              placeholder="My Portfolio"
              required
            />
          </UFormField>

          <UFormField
            name="password"
            label="Password"
            :required="true"
            :errors="errors"
          >
            <PasswordInput
              v-model="request.password"
              placeholder="Create a password"
              autocomplete="new-password"
              required
            />
          </UFormField>

          <UFormField
            name="confirmPassword"
            label="Confirm password"
            :required="true"
            :errors="errors"
          >
            <PasswordInput
              v-model="request.confirmPassword"
              placeholder="Confirm your password"
              autocomplete="new-password"
              required
            />
          </UFormField>

          <Button
            type="submit"
            variant="default"
            size="lg"
            class="w-full"
            @click="submit"
          >
            Create account
          </Button>
        </div>

        <UDivider text="or" />

        <!-- Social login -->
        <Button variant="outline" class="w-full">
          <Icon name="logos:github-icon" class="w-4 h-4 mr-2" />
          Continue with GitHub
        </Button>

        <!-- Login link -->
        <p class="text-center text-sm text-muted-foreground mt-6">
          Already have an account?
          <NuxtLink to="/auth/login" class="text-primary hover:text-primary/80 font-medium transition-colors">
            Log in
          </NuxtLink>
        </p>
      </div>
    </UCard>
  </div>
</template>

<script setup lang="ts">
import { registerSchema, type RegisterFormData } from './models/register'
import type { RegisterRequest } from '~/api/portfolio.api.generated.clients'
import { useValidate } from '~/composables/useValidate'
import { Button } from '~/components/ui/button'
import { Input, PasswordInput } from '~/components/ui/input'

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
