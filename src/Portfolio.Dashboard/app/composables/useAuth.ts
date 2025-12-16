import { apiFactory } from '~/api/portfolio.api'
import { AuthClient } from '~/api/portfolio.api.generated.clients'
import type { AuthUserDto, ForgotPasswordRequest, LoginRequest, RegisterRequest, ResetPasswordRequest } from '~/api/portfolio.api.generated.clients'

export const useAuth = () => {
  const user = useState<AuthUserDto | null>('auth:user', () => null)
  const isLoading = useState<boolean>('auth:loading', () => false)
  const isInitialized = useState<boolean>('auth:initialized', () => false)

  const authClient = apiFactory.create(AuthClient)

  const login = async (request: LoginRequest) => {
    isLoading.value = true
    try {
      const result = await authClient.login(request)
      user.value = result

      if (!result.isEmailConfirmed) {
        await navigateTo('/auth/verify-email')
      }
      else {
        await navigateTo('/accounts')
      }
    }
    finally {
      isLoading.value = false
    }
  }

  const logout = async () => {
    isLoading.value = true
    try {
      await authClient.logout()
      user.value = null
      await navigateTo('/auth/login')
    }
    finally {
      isLoading.value = false
    }
  }

  const fetchUser = async () => {
    console.log('test')

    isLoading.value = true
    try {
      const response = await authClient.getCurrentUser()
      user.value = response
      return response
    }
    catch (error: any) {
      if (error.status === 401) {
        user.value = null
      }
      throw error
    }
    finally {
      isLoading.value = false
      isInitialized.value = true
    }
  }

  const register = async (request: RegisterRequest) => {
    isLoading.value = true
    try {
      const result = await authClient.register(request)
      user.value = result
      await navigateTo('/auth/verify-email')
    }
    finally {
      isLoading.value = false
    }
  }

  const forgotPassword = async (request: ForgotPasswordRequest) => {
    await authClient.forgotPassword(request)
  }

  const resetPassword = async (request: ResetPasswordRequest) => {
    await authClient.resetPassword(request)
  }

  return {
    user,
    isLoading,
    isInitialized,
    login,
    logout,
    fetchUser,
    register,
    forgotPassword,
    resetPassword,
  }
}
