<template>
  <div class="w-full max-w-sm">
    <BaseCard variant="glass">
      <!-- Header -->
      <div class="text-center mb-6">
        <h1 class="text-xl font-semibold text-gray-900">
          Log in
        </h1>
        <p class="text-gray-500 mt-1 text-sm">
          Continue to Portfolio
        </p>
      </div>

      <!-- Form -->
      <div class="space-y-4">
        <FormField
          name="email"
          label="Email"
          :required="true"
          :errors="errors"
          variant="glass"
        >
          <BaseInput
            v-model="request.email"
            type="email"
            placeholder="Enter your email"
            autocomplete="email"
            variant="glass"
            required
          />
        </FormField>

        <FormField
          name="password"
          label="Password"
          :required="true"
          :errors="errors"
          variant="glass"
        >
          <BaseInput
            v-model="request.password"
            type="password"
            placeholder="Enter your password"
            autocomplete="current-password"
            variant="glass"
            required
          />
        </FormField>

        <div class="flex items-center justify-end">
          <BaseLink to="/auth/forgot-password" variant="glass" class="text-sm">
            Forgot password?
          </BaseLink>
        </div>

        <BaseButton
          type="submit"
          variant="primary"
          size="lg"
          full-width
          @click="submit"
        >
          Log in
        </BaseButton>
      </div>

      <BaseDivider text="or" variant="glass" />

      <!-- Social login buttons -->
      <BaseButton variant="outline-glass" full-width>
        <Icon name="logos:github-icon" class="w-4 h-4 mr-2" />
        Continue with GitHub
      </BaseButton>

      <!-- Register link -->
      <p class="text-center text-sm text-gray-500 mt-6">
        New to Portfolio?
        <BaseLink to="/auth/register" variant="glass">
          Create an account
        </BaseLink>
      </p>
    </BaseCard>
  </div>
</template>

<script setup lang="ts">
import z from 'zod'
import { LoginRequest } from '~/api/portfolio.api.generated.clients'
import { useValidate } from '~/composables/useValidate'
import FormField from '~/components/FormField.vue'
import { useProblemFormErrors } from '~/composables/useApiFormErrors'

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
