<template>
  <div class="w-full max-w-sm">
    <BaseCard variant="glass">
      <!-- Success State -->
      <template v-if="submitted">
        <div class="text-center">
          <div class="w-12 h-12 bg-blue-100 dark:bg-blue-900/30 rounded-full flex items-center justify-center mx-auto mb-4">
            <Icon name="lucide:mail-check" class="w-6 h-6 text-blue-600 dark:text-blue-400" />
          </div>
          <h1 class="text-xl font-semibold text-gray-900 mb-2">
            Check your email
          </h1>
          <p class="text-gray-500 text-sm mb-4">
            We've sent a password reset link to<br>
            <span class="font-medium text-gray-900">{{ request.email }}</span>
          </p>
          <p class="text-xs text-gray-500 mb-4">
            Didn't receive the email? Check your spam folder or
            <button
              type="button"
              class="text-blue-600 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300 font-medium hover:underline"
              @click="submitted = false"
            >
              try again
            </button>
          </p>
          <BaseButton
            variant="outline-glass"
            full-width
            @click="navigateTo('/auth/login')"
          >
            <Icon name="lucide:arrow-left" class="w-4 h-4 mr-2" />
            Back to log in
          </BaseButton>
        </div>
      </template>

      <!-- Form State -->
      <template v-else>
        <!-- Header -->
        <div class="text-center mb-6">
          <div class="w-12 h-12 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <Icon name="lucide:key" class="w-6 h-6 text-gray-600" />
          </div>
          <h1 class="text-xl font-semibold text-gray-900">
            Reset your password
          </h1>
          <p class="text-gray-500 mt-1 text-sm">
            Enter your email to receive reset instructions
          </p>
        </div>

        <!-- Error Alert -->
        <BaseAlert
          v-if="formError"
          variant="error"
          dismissible
          glass
          class="mb-4"
          @dismiss="clearError"
        >
          {{ formError }}
        </BaseAlert>

        <!-- Form -->
        <div class="space-y-4">
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
              label="Email"
              placeholder="Enter your email"
              autocomplete="email"
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
            Send reset link
          </BaseButton>
        </div>

        <!-- Back to login -->
        <p class="text-center text-sm text-gray-500 mt-4">
          <BaseLink to="/auth/login" variant="glass" class="inline-flex items-center gap-1">
            <Icon name="lucide:arrow-left" class="w-4 h-4" />
            Back to log in
          </BaseLink>
        </p>
      </template>
    </BaseCard>
  </div>
</template>

<script setup lang="ts">
import z from 'zod'
import apiFactory from '~/api/portfolio.api'
import { AuthClient, ForgotPasswordRequest } from '~/api/portfolio.api.generated.clients'

definePageMeta({
  layout: 'auth',
})

const authClient = apiFactory.create(AuthClient)
const request = ref<ForgotPasswordRequest>(new ForgotPasswordRequest())
const submitted = ref(false)

const forgotPasswordSchema = z.object({
  email: z.string().min(1, 'validation.required').email('validation.email'),
})
const { validate, errors, addFromProblem } = useValidate(forgotPasswordSchema)
const { formError, clearFormError, applyError } = useProblemFormErrors(addFromProblem)

async function submit() {
  clearFormError()
  if (!validate(request.value)) return

  try {
    await authClient.forgotPassword(request.value)
    submitted.value = true
  }
  catch (e) {
    applyError(e)
  }
}
</script>
