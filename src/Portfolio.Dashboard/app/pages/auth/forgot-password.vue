<template>
  <div class="w-full max-w-md">
    <UCard>
      <div class="p-8">
        <!-- Success State -->
        <template v-if="submitted">
          <div class="text-center">
            <div class="w-16 h-16 bg-primary/10 rounded-full flex items-center justify-center mx-auto mb-4">
              <Icon name="lucide:mail-check" class="w-8 h-8 text-primary" />
            </div>
            <h1 class="text-2xl font-semibold text-foreground mb-2">
              Check your email
            </h1>
            <p class="text-muted-foreground mb-1">
              We've sent a password reset link to
            </p>
            <p class="font-medium text-foreground mb-4">
              {{ request.email }}
            </p>
            <p class="text-sm text-muted-foreground mb-6">
              Didn't receive the email? Check your spam folder or
              <button
                type="button"
                class="text-primary hover:text-primary/80 font-medium transition-colors"
                @click="submitted = false"
              >
                try again
              </button>
            </p>
            <Button
              variant="outline"
              size="lg"
              class="w-full"
              as-child
            >
              <NuxtLink to="/auth/login">
                <Icon name="lucide:arrow-left" class="w-4 h-4 mr-2" />
                Back to log in
              </NuxtLink>
            </Button>
          </div>
        </template>

        <!-- Form State -->
        <template v-else>
          <!-- Header -->
          <div class="mb-8">
            <div class="w-16 h-16 bg-muted rounded-full flex items-center justify-center mx-auto mb-4">
              <Icon name="lucide:key" class="w-8 h-8 text-muted-foreground" />
            </div>
            <h1 class="text-2xl font-semibold text-foreground text-center">
              Reset your password
            </h1>
            <p class="text-muted-foreground mt-2 text-center">
              Enter your email to receive reset instructions
            </p>
          </div>

          <!-- Error Alert -->
          <UAlert
            v-if="formError"
            variant="error"
            dismissible
            class="mb-6"
            @dismiss="clearFormError"
          >
            {{ formError }}
          </UAlert>

          <!-- Form -->
          <div class="space-y-4">
            <UFormField
              name="email"
              label="Email"
              :required="true"
              :error="errors.email"
            >
              <Input
                v-model="request.email"
                type="email"
                placeholder="you@example.com"
                autocomplete="email"
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
              Send reset link
            </Button>
          </div>

          <!-- Back to login -->
          <p class="text-center text-sm text-muted-foreground mt-6">
            <NuxtLink to="/auth/login" class="inline-flex items-center gap-1 text-primary hover:text-primary/80 transition-colors">
              <Icon name="lucide:arrow-left" class="w-4 h-4" />
              Back to log in
            </NuxtLink>
          </p>
        </template>
      </div>
    </UCard>
  </div>
</template>

<script setup lang="ts">
import z from 'zod'
import apiFactory from '~/api/portfolio.api'
import { AuthClient, ForgotPasswordRequest } from '~/api/portfolio.api.generated.clients'
import { Button } from '~/components/ui/button'
import { Input } from '~/components/ui/input'

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
