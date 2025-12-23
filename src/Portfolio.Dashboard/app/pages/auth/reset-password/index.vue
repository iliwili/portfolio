<template>
  <div class="w-full max-w-sm">
    <UCard>
      <div class="p-8">
        <!-- Invalid Token State -->
        <template v-if="invalidToken">
          <div class="text-center">
            <div class="w-12 h-12 bg-destructive/10 rounded-full flex items-center justify-center mx-auto mb-4">
              <Icon name="lucide:link-2-off" class="w-6 h-6 text-destructive" />
            </div>
            <h1 class="text-xl font-semibold text-foreground mb-2">
              Invalid reset link
            </h1>
            <p class="text-muted-foreground text-sm mb-4">
              This password reset link is invalid or has expired.
            </p>
            <Button
              variant="default"
              size="lg"
              class="w-full"
              @click="navigateTo('/auth/forgot-password')"
            >
              Request new link
            </Button>
          </div>
        </template>

        <!-- Success State -->
        <template v-else-if="success">
          <div class="text-center">
            <div class="w-12 h-12 bg-primary/10 rounded-full flex items-center justify-center mx-auto mb-4">
              <Icon name="lucide:check-circle" class="w-6 h-6 text-primary" />
            </div>
            <h1 class="text-xl font-semibold text-foreground mb-2">
              Password updated
            </h1>
            <p class="text-muted-foreground text-sm mb-4">
              Your password has been successfully updated.
            </p>
            <Button
              variant="default"
              size="lg"
              class="w-full"
              @click="navigateTo('/auth/login')"
            >
              Log in
            </Button>
          </div>
        </template>

        <!-- Form State -->
        <template v-else>
          <!-- Header -->
          <div class="text-center mb-6">
            <div class="w-12 h-12 bg-muted rounded-full flex items-center justify-center mx-auto mb-4">
              <Icon name="lucide:lock" class="w-6 h-6 text-muted-foreground" />
            </div>
            <h1 class="text-xl font-semibold text-foreground">
              Set new password
            </h1>
            <p class="text-muted-foreground mt-1 text-sm">
              Choose a strong password for your account
            </p>
          </div>

          <!-- Form -->
          <div class="space-y-4">
            <UFormField
              name="newPassword"
              label="New password"
              :required="true"
              :errors="errors"
            >
              <PasswordInput
                v-model="request.newPassword"
                placeholder="Enter new password"
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
                placeholder="Confirm new password"
                autocomplete="new-password"
                required
              />
            </UFormField>

            <Button
              type="submit"
              variant="default"
              size="lg"
              class="w-full"
              :disabled="isLoading"
              @click="submit"
            >
              <Icon v-if="isLoading" name="lucide:loader-2" class="mr-2 h-4 w-4 animate-spin" />
              Reset password
            </Button>
          </div>

          <!-- Back to login -->
          <p class="text-center text-sm text-muted-foreground mt-6">
            <NuxtLink to="/auth/login" class="inline-flex items-center gap-1 text-primary hover:text-primary/80 transition-colors">
              <Icon name="lucide:arrow-left" class="w-4 h-4" />
              Back to sign in
            </NuxtLink>
          </p>
        </template>
      </div>
    </UCard>
  </div>
</template>

<script setup lang="ts">
import { ResetPasswordRequest } from '~/api/portfolio.api.generated.clients'
import { resetPasswordSchema, type ResetPasswordFormData } from './models/resetPassword'
import { Button } from '~/components/ui/button'
import { PasswordInput } from '~/components/ui/input'

definePageMeta({
  layout: 'auth',
})

const route = useRoute()
const { resetPassword } = useAuth()

const request = ref<ResetPasswordFormData>({
  newPassword: '',
  confirmPassword: '',
})

const { validate, errors, addFromProblem } = useValidate(resetPasswordSchema)
const { clearFormError, applyError } = useProblemFormErrors(addFromProblem)

const token = computed(() => route.query.token as string || '')
const success = ref(false)
const isLoading = ref(false)

const invalidToken = computed(() => !token.value)

async function submit() {
  clearFormError()
  if (!validate(request.value)) return

  isLoading.value = true
  try {
    const requestBody = new ResetPasswordRequest()
    requestBody.token = token.value
    requestBody.newPassword = request.value.newPassword
    await resetPassword(requestBody)
    success.value = true
  }
  catch (e) {
    applyError(e)
    success.value = false
  }
  finally {
    isLoading.value = false
  }
}
</script>
