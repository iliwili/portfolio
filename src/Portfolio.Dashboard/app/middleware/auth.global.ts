export default defineNuxtRouteMiddleware((to) => {
  const { user, isInitialized } = useAuth()

  // Public routes that don't require auth
  const publicRoutes = [
    '/auth/login',
    '/auth/register',
    '/auth/forgot-password',
    '/auth/reset-password',
  ]

  const isPublicRoute = publicRoutes.some(route => to.path.startsWith(route))

  // Wait for auth to initialize before redirecting
  if (!isInitialized.value) {
    // Plugin hasn't finished checking auth yet
    return
  }

  // If not logged in and trying to access protected route
  if (!user.value && !isPublicRoute) {
    return navigateTo('/auth/login')
  }

  // If logged in and trying to access login page
  if (user.value && to.path === '/auth/login') {
    return navigateTo('/accounts')
  }

  // Check email verification for protected routes
  if (user.value && !user.value.isEmailConfirmed && to.path !== '/auth/verify-email') {
    return navigateTo('/auth/verify-email')
  }
})
