import { apiFactory } from '~/api/portfolio.api'
import { AuthClient } from '~/api/portfolio.api.generated.clients'
import type { AuthUserDto, ForgotPasswordRequest, LoginRequest, RegisterRequest, ResendVerificationEmailRequest, ResetPasswordRequest, VerifyEmailRequest } from '~/api/portfolio.api.generated.clients'

export const useAuth = () => {
  const user = useState<AuthUserDto | null>('auth:user', () => null)

  const authClient = apiFactory.create(AuthClient)

  const login = async (request: LoginRequest) => {
    const result = await authClient.login(request)
    user.value = result

    if (!result.isEmailConfirmed) {
      await navigateTo('/auth/verify-email')
    }
    else {
      await navigateTo('/accounts')
    }
  }

  const logout = async () => {
    await authClient.logout()
    user.value = null
    await navigateTo('/auth/login')
  }

  const fetchUser = async () => {
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
  }

  const register = async (request: RegisterRequest) => {
    const result = await authClient.register(request)
    user.value = result
    await navigateTo('/auth/verify-email')
  }

  const resendVerificationEmail = async (request: ResendVerificationEmailRequest) => {
    await authClient.resendVerification(request)
  }

  const verifyEmail = async (request: VerifyEmailRequest) => {
    await authClient.verifyEmail(request)
  }

  const forgotPassword = async (request: ForgotPasswordRequest) => {
    await authClient.forgotPassword(request)
  }

  const resetPassword = async (request: ResetPasswordRequest) => {
    await authClient.resetPassword(request)
  }

  return {
    user,
    login,
    logout,
    fetchUser,
    register,
    forgotPassword,
    resetPassword,
    resendVerificationEmail,
    verifyEmail,
  }
}
