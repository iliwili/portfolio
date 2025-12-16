<template>
  <div class="w-full max-w-sm">
    <BaseCard variant="glass">
      <!-- Verifying State -->
      <template v-if="verificationStatus === 'verifying'">
        <div class="text-center">
          <div class="w-12 h-12 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <Icon name="lucide:loader-2" class="w-6 h-6 text-gray-600 animate-spin" />
          </div>
          <h1 class="text-xl font-semibold text-gray-900 mb-2">
            Verifying your email
          </h1>
          <p class="text-gray-500 text-sm">
            Please wait...
          </p>
        </div>
      </template>

      <!-- Success State -->
      <template v-else-if="verificationStatus === 'success'">
        <div class="text-center">
          <div class="w-12 h-12 bg-blue-100 dark:bg-blue-900/30 rounded-full flex items-center justify-center mx-auto mb-4">
            <Icon name="lucide:check-circle" class="w-6 h-6 text-blue-600 dark:text-blue-400" />
          </div>
          <h1 class="text-xl font-semibold text-gray-900 mb-2">
            Email verified
          </h1>
          <p class="text-gray-500 text-sm mb-4">
            Your email has been verified successfully.
          </p>
          <BaseButton
            variant="primary"
            full-width
            @click="navigateTo('/accounts')"
          >
            Continue
          </BaseButton>
        </div>
      </template>

      <!-- Error State (from token verification) -->
      <template v-else-if="verificationStatus === 'error'">
        <div class="text-center">
          <div class="w-12 h-12 bg-red-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <Icon name="lucide:x-circle" class="w-6 h-6 text-red-600" />
          </div>
          <h1 class="text-xl font-semibold text-gray-900 mb-2">
            Verification failed
          </h1>
          <p class="text-gray-500 text-sm mb-4">
            {{ error || 'The verification link is invalid or has expired.' }}
          </p>

          <div class="space-y-2">
            <BaseButton
              v-if="user?.email"
              variant="primary"
              full-width
              :loading="isLoading"
              @click="handleResend"
            >
              Resend verification email
            </BaseButton>
            <BaseButton
              variant="outline-glass"
              full-width
              @click="navigateTo('/auth/login')"
            >
              Back to log in
            </BaseButton>
          </div>
        </div>
      </template>

      <!-- Pending State (no token, waiting for verification) -->
      <template v-else>
        <div class="text-center">
          <div class="w-12 h-12 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <Icon name="lucide:mail" class="w-6 h-6 text-gray-600" />
          </div>
          <h1 class="text-xl font-semibold text-gray-900 mb-2">
            Verify your email
          </h1>
          <p class="text-gray-500 text-sm mb-1">
            We've sent a verification link to
          </p>
          <p v-if="user?.email" class="font-medium text-gray-900 text-sm mb-4">
            {{ user.email }}
          </p>
          <p class="text-xs text-gray-500 mb-4">
            Click the link in your email to verify your account.
          </p>

          <!-- Success message for resend -->
          <BaseAlert
            v-if="resendSuccess"
            variant="success"
            glass
            class="mb-4"
          >
            Verification email sent!
          </BaseAlert>

          <!-- Error message -->
          <BaseAlert
            v-if="error && !resendSuccess"
            variant="error"
            dismissible
            glass
            class="mb-4"
            @dismiss="clearError"
          >
            {{ error }}
          </BaseAlert>

          <div class="space-y-2">
            <BaseButton
              variant="primary"
              full-width
              :loading="isLoading"
              @click="handleResend"
            >
              Resend verification email
            </BaseButton>
            <BaseButton
              variant="outline-glass"
              full-width
              @click="navigateTo('/auth/login')"
            >
              <Icon name="lucide:arrow-left" class="w-4 h-4 mr-2" />
              Back to log in
            </BaseButton>
          </div>
        </div>
      </template>
    </BaseCard>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: 'auth',
})

const route = useRoute()
const { verifyEmail, resendVerificationEmail, isLoading, error, clearError, user, isEmailVerified } = useAuth()

const token = computed(() => route.query.token as string || '')
const verificationStatus = ref<'pending' | 'verifying' | 'success' | 'error'>('pending')
const resendSuccess = ref(false)

// Auto-verify if token is present
onMounted(async () => {
  if (token.value) {
    verificationStatus.value = 'verifying'
    const success = await verifyEmail(token.value)
    verificationStatus.value = success ? 'success' : 'error'
  }
})

// Watch for email verification in user state
watch(isEmailVerified, (verified) => {
  if (verified) {
    verificationStatus.value = 'success'
  }
})

async function handleResend() {
  clearError()
  resendSuccess.value = false
  const success = await resendVerificationEmail()
  if (success) {
    resendSuccess.value = true
  }
}
</script>
