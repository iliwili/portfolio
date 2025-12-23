<template>
  <div class="w-full max-w-md">
    <UCard>
      <div class="p-8">
        <!-- Header -->
        <div class="mb-8">
          <h1 class="text-2xl font-semibold text-foreground">
            Log in
          </h1>
          <p class="text-muted-foreground mt-1">
            Continue to Portfolio
          </p>
        </div>

        <!-- Form -->
        <div class="space-y-4">
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
            name="password"
            label="Password"
            :required="true"
            :errors="errors"
          >
            <PasswordInput
              v-model="request.password"
              placeholder="Enter your password"
              autocomplete="current-password"
              required
            />
          </UFormField>

          <div class="flex items-center justify-end">
            <NuxtLink to="/auth/forgot-password" class="text-sm text-primary hover:text-primary/80 transition-colors">
              Forgot password?
            </NuxtLink>
          </div>

          <Button
            class="w-full"
            type="submit"
            variant="default"
            size="lg"
            full-width
            @click="submit"
          >
            Log in
          </Button>
        </div>

        <UDivider text="or" />

        <!-- Social login -->
        <Button class="w-full" variant="outline">
          <Icon name="logos:github-icon" class="w-4 h-4 mr-2" />
          Continue with GitHub
        </Button>

        <!-- Register link -->
        <p class="text-center text-sm text-muted-foreground mt-6">
          New to Portfolio?
          <NuxtLink to="/auth/register" class="text-primary hover:text-primary/80 font-medium transition-colors">
            Create an account
          </NuxtLink>
        </p>
      </div>
    </UCard>
  </div>
</template>

<script setup lang="ts">
import z from 'zod'
import { LoginRequest } from '~/api/portfolio.api.generated.clients'
import { useValidate } from '~/composables/useValidate'
import { useProblemFormErrors } from '~/composables/useApiFormErrors'
import { Button } from '~/components/ui/button'
import { Input, PasswordInput } from '~/components/ui/input'

definePageMeta({
  layout: 'auth',
})

const { login } = useAuth()
const request = ref<LoginRequest>(new LoginRequest())

const loginSchema = z.object({
  email: z.string().min(1, 'validation.required').email('validation.email'),
  password: z.string().min(1, 'validation.required').min(8, 'validation.minLength'),
})
const { validate, errors, addFromProblem } = useValidate(loginSchema)
const { clearFormError, applyError } = useProblemFormErrors(addFromProblem)

async function submit() {
  clearFormError()
  if (!validate(request.value)) return

  try {
    await login(request.value)
  }
  catch (e) {
    applyError(e)
  }
}
</script>
