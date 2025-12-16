<template>
  <div />
</template>

<script setup lang="ts">
// Redirect to accounts page (middleware will handle auth check)
definePageMeta({
  layout: false,
})

await navigateTo('/accounts')
</script>
