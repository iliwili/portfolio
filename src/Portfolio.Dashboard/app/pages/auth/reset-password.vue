<template>
  <div class="w-full max-w-sm">
    <BaseCard variant="glass">
      <!-- Invalid Token State -->
      <template v-if="invalidToken">
        <div class="text-center">
          <div class="w-12 h-12 bg-red-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <Icon name="lucide:link-2-off" class="w-6 h-6 text-red-600" />
          </div>
          <h1 class="text-xl font-semibold text-gray-900 mb-2">
            Invalid reset link
          </h1>
          <p class="text-gray-500 text-sm mb-4">
            This password reset link is invalid or has expired.
          </p>
          <BaseButton
            variant="primary"
            full-width
            @click="navigateTo('/auth/forgot-password')"
          >
            Request new link
          </BaseButton>
        </div>
      </template>

      <!-- Success State -->
      <template v-else-if="success">
        <div class="text-center">
          <div class="w-12 h-12 bg-blue-100 dark:bg-blue-900/30 rounded-full flex items-center justify-center mx-auto mb-4">
            <Icon name="lucide:check-circle" class="w-6 h-6 text-blue-600 dark:text-blue-400" />
          </div>
          <h1 class="text-xl font-semibold text-gray-900 mb-2">
            Password updated
          </h1>
          <p class="text-gray-500 text-sm mb-4">
            Your password has been successfully updated.
          </p>
          <BaseButton
            variant="primary"
            full-width
            @click="navigateTo('/auth/login')"
          >
            Log in
          </BaseButton>
        </div>
      </template>

      <!-- Form State -->
      <template v-else>
        <!-- Header -->
        <div class="text-center mb-6">
          <div class="w-12 h-12 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <Icon name="lucide:lock" class="w-6 h-6 text-gray-600" />
          </div>
          <h1 class="text-xl font-semibold text-gray-900">
            Set new password
          </h1>
          <p class="text-gray-500 mt-1 text-sm">
            Choose a strong password for your account
          </p>
        </div>

        <!-- Error Alert -->
        <BaseAlert
          v-if="error"
          variant="error"
          dismissible
          glass
          class="mb-4"
          @dismiss="clearError"
        >
          {{ error }}
        </BaseAlert>

        <!-- Form -->
        <form class="space-y-4" @submit.prevent="handleSubmit">
          <BaseInput
            v-model="password"
            type="password"
            label="New password"
            placeholder="Enter new password"
            autocomplete="new-password"
            variant="glass"
            required
          />

          <BaseInput
            v-model="confirmPassword"
            type="password"
            label="Confirm password"
            placeholder="Confirm new password"
            autocomplete="new-password"
            variant="glass"
            :error="passwordMismatch ? 'Passwords do not match' : ''"
            required
          />

          <BaseButton
            type="submit"
            variant="primary"
            size="lg"
            full-width
            :loading="isLoading"
            :disabled="passwordMismatch"
          >
            Reset password
          </BaseButton>
        </form>

        <!-- Back to login -->
        <p class="text-center text-sm text-gray-500 mt-6">
          <BaseLink to="/auth/login" variant="glass" class="inline-flex items-center gap-1">
            <Icon name="lucide:arrow-left" class="w-4 h-4" />
            Back to sign in
          </BaseLink>
        </p>
      </template>
    </BaseCard>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: 'auth',
})

const route = useRoute()
const { resetPassword } = useAuth()

const token = computed(() => route.query.token as string || '')
const password = ref('')
const confirmPassword = ref('')
const success = ref(false)
const isLoading = ref(false)
const error = ref('')

const passwordMismatch = computed(() => {
  return Boolean(confirmPassword.value && password.value !== confirmPassword.value)
})

const invalidToken = computed(() => !token.value)

function clearError() {
  error.value = ''
}

async function handleSubmit() {
  if (passwordMismatch.value || invalidToken.value)
    return

  clearError()
  isLoading.value = true
  try {
    const result = await resetPassword(token.value, password.value)
    if (result) {
      success.value = true
    }
  }
  catch (e: any) {
    error.value = e?.message || 'Failed to reset password. Please try again.'
  }
  finally {
    isLoading.value = false
  }
}
</script>
