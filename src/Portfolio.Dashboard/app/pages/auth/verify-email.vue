<template>
  <div class="w-full max-w-md">
    <UCard>
      <div class="p-8">
        <!-- Verifying -->
        <template v-if="verificationStatus === 'verifying'">
          <div class="text-center">
            <div class="w-16 h-16 bg-muted rounded-full flex items-center justify-center mx-auto mb-4">
              <Icon name="lucide:loader-2" class="w-8 h-8 text-primary animate-spin" />
            </div>
            <h1 class="text-2xl font-semibold text-foreground mb-2">
              Verifying your email
            </h1>
            <p class="text-muted-foreground">
              Please wait...
            </p>
          </div>
        </template>

        <!-- Success -->
        <template v-else-if="verificationStatus === 'success'">
          <div class="text-center">
            <div class="w-16 h-16 bg-primary/10 rounded-full flex items-center justify-center mx-auto mb-4">
              <Icon name="lucide:check-circle" class="w-8 h-8 text-primary" />
            </div>
            <h1 class="text-2xl font-semibold text-foreground mb-2">
              Email verified
            </h1>
            <p class="text-muted-foreground mb-6">
              Your email has been verified successfully.
            </p>
            <Button
              variant="default"
              size="lg"
              class="w-full"
              as-child
            >
              <NuxtLink to="/accounts">
                Continue
              </NuxtLink>
            </Button>
          </div>
        </template>

        <!-- Error -->
        <template v-else-if="verificationStatus === 'error'">
          <div class="text-center">
            <div class="w-16 h-16 bg-destructive/10 rounded-full flex items-center justify-center mx-auto mb-4">
              <Icon name="lucide:x-circle" class="w-8 h-8 text-destructive" />
            </div>
            <h1 class="text-2xl font-semibold text-foreground mb-2">
              Verification failed
            </h1>
            <p class="text-muted-foreground mb-6">
              {{ error || 'The verification link is invalid or has expired.' }}
            </p>

            <div class="space-y-2">
              <Button
                v-if="user?.email"
                variant="default"
                size="lg"
                class="w-full"
                @click="resend"
              >
                Resend verification email
              </Button>
              <Button
                variant="outline"
                size="lg"
                class="w-full"
                as-child
              >
                <NuxtLink to="/auth/login">
                  Back to log in
                </NuxtLink>
              </Button>
            </div>
          </div>
        </template>

        <!-- Pending -->
        <template v-else>
          <div class="text-center">
            <div class="w-16 h-16 bg-muted rounded-full flex items-center justify-center mx-auto mb-4">
              <Icon name="lucide:mail" class="w-8 h-8 text-muted-foreground" />
            </div>
            <h1 class="text-2xl font-semibold text-foreground mb-2">
              Verify your email
            </h1>
            <p class="text-slate-400 mb-1">
              We've sent a verification link to
            </p>
            <p v-if="user?.email" class="font-medium text-foreground mb-4">
              {{ user.email }}
            </p>
            <p class="text-sm text-muted-foreground mb-6">
              Click the link in your email to verify your account.
            </p>

            <UAlert
              v-if="resendSuccess"
              variant="success"
              class="mb-4"
            >
              Verification email sent!
            </UAlert>

            <UAlert
              v-if="error && !resendSuccess"
              variant="error"
              dismissible
              class="mb-4"
              @dismiss="clearError"
            >
              {{ error }}
            </UAlert>

            <div class="space-y-2">
              <Button
                variant="default"
                size="lg"
                class="w-full"
                @click="resend"
              >
                Resend verification email
              </Button>
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
          </div>
        </template>
      </div>
    </UCard>
  </div>
</template>

<script setup lang="ts">
import { ResendVerificationEmailRequest, VerifyEmailRequest } from '~/api/portfolio.api.generated.clients'
import { Button } from '~/components/ui/button'

definePageMeta({
  layout: 'auth',
})

const route = useRoute()
const { user, verifyEmail, resendVerificationEmail } = useAuth()

const token = computed(() => route.query.token as string || '')
const verificationStatus = ref<'pending' | 'verifying' | 'success' | 'error'>('pending')
const resendSuccess = ref(false)
const error = ref('')

watch(token, async (newToken) => {
  if (newToken) {
    verificationStatus.value = 'verifying'
    const success = await verifyToken(newToken)
    verificationStatus.value = success ? 'success' : 'error'
  }
}, { immediate: true })

async function verifyToken(token: string): Promise<boolean> {
  try {
    if (user.value?.isEmailConfirmed) {
      navigateTo('/accounts')
    }

    const request = new VerifyEmailRequest()
    request.token = token

    await verifyEmail(request)
    return true
  }
  catch (e: any) {
    error.value = e?.errorCode || 'The verification link is invalid or has expired.'
    return false
  }
}

async function resend() {
  clearError()
  try {
    const request = new ResendVerificationEmailRequest()
    request.email = user.value?.email || ''

    await resendVerificationEmail(request)
    resendSuccess.value = true
  }
  catch (e: any) {
    error.value = e?.message || 'Failed to resend verification email.'
  }
}

function clearError() {
  error.value = ''
  resendSuccess.value = false
}
</script>
