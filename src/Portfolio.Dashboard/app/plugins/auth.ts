export default defineNuxtPlugin(async () => {
  const { fetchUser } = useAuth()

  console.log('testerrr')
  // Only run on client-side
  if (import.meta.client) {
    try {
      // Try to fetch user from cookie on app load
      await fetchUser()
    }
    catch {
      // User not logged in or cookie expired - that's okay
      console.log('No active session')
    }
  }
})
