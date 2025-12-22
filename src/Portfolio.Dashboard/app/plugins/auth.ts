export default defineNuxtPlugin(async () => {
  const { fetchUser } = useAuth()

  // Only run on client-side
  if (import.meta.client) {
    try {
      // Try to fetch user from cookie on app load
      console.log('tester')
      await fetchUser()
    }
    catch {
      // User not logged in or cookie expired - that's okay
      console.log('No active session')
      await navigateTo('/auth/login')
    }
  }
})
