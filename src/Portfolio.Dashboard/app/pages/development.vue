<script setup lang="ts">
definePageMeta({
  layout: 'default',
})

const email = ref('')
const password = ref('')
const selectedOption = ref('')
const dialogOpen = ref(false)

const selectOptions = [
  { value: 'option1', label: 'Option 1' },
  { value: 'option2', label: 'Option 2' },
  { value: 'option3', label: 'Option 3', disabled: true },
  { value: 'option4', label: 'Option 4' },
]

const dropdownItems = [
  { label: 'Edit', icon: 'lucide:edit', onClick: () => console.log('Edit clicked') },
  { label: 'Duplicate', icon: 'lucide:copy', onClick: () => console.log('Duplicate clicked') },
  { separator: true },
  { label: 'Archive', icon: 'lucide:archive', onClick: () => console.log('Archive clicked') },
  { label: 'Delete', icon: 'lucide:trash', onClick: () => console.log('Delete clicked'), disabled: true },
]
</script>

<template>
  <div class="max-w-7xl mx-auto p-6 space-y-12">
    <!-- Header -->
    <div class="border-b border-gray-200 dark:border-gray-700 pb-6">
      <h1 class="text-3xl font-bold text-gray-900 dark:text-white mb-2">
        Component Library
      </h1>
      <p class="text-gray-600 dark:text-gray-400">
        All base components with variants and states
      </p>
    </div>

    <!-- Buttons -->
    <section class="space-y-4">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white">
        Buttons
      </h2>

      <div class="space-y-6">
        <!-- Variants -->
        <div>
          <h3 class="text-sm font-medium text-gray-700 dark:text-gray-300 mb-3">
            Variants
          </h3>
          <div class="flex flex-wrap gap-3">
            <BaseButton variant="primary">
              Primary
            </BaseButton>
            <BaseButton variant="secondary">
              Secondary
            </BaseButton>
            <BaseButton variant="outline">
              Outline
            </BaseButton>
            <BaseButton variant="outline-glass">
              Outline Glass
            </BaseButton>
            <BaseButton variant="ghost">
              Ghost
            </BaseButton>
            <BaseButton variant="danger">
              Danger
            </BaseButton>
            <BaseButton variant="success">
              Success
            </BaseButton>
          </div>
        </div>

        <!-- Sizes -->
        <div>
          <h3 class="text-sm font-medium text-gray-700 dark:text-gray-300 mb-3">
            Sizes
          </h3>
          <div class="flex items-center flex-wrap gap-3">
            <BaseButton size="sm">
              Small
            </BaseButton>
            <BaseButton size="md">
              Medium
            </BaseButton>
            <BaseButton size="lg">
              Large
            </BaseButton>
          </div>
        </div>

        <!-- States -->
        <div>
          <h3 class="text-sm font-medium text-gray-700 dark:text-gray-300 mb-3">
            States
          </h3>
          <div class="flex flex-wrap gap-3">
            <BaseButton :loading="true">
              Loading
            </BaseButton>
            <BaseButton :disabled="true">
              Disabled
            </BaseButton>
            <BaseButton full-width>
              Full Width
            </BaseButton>
          </div>
        </div>
      </div>
    </section>

    <!-- Inputs -->
    <section class="space-y-4">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white">
        Inputs
      </h2>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-6 max-w-4xl">
        <BaseInput
          v-model="email"
          type="email"
          label="Email"
          placeholder="Enter your email"
          hint="We'll never share your email"
        />

        <BaseInput
          v-model="password"
          type="password"
          label="Password"
          placeholder="Enter password"
        />

        <BaseInput
          v-model="email"
          type="email"
          label="With Error"
          placeholder="Enter email"
          error="Email is required"
        />

        <BaseInput
          v-model="email"
          type="email"
          label="Disabled"
          placeholder="Disabled input"
          :disabled="true"
        />

        <BaseInput
          v-model="email"
          type="email"
          label="Glass Variant"
          placeholder="Enter email"
          variant="glass"
        />
      </div>
    </section>

    <!-- Select -->
    <section class="space-y-4">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white">
        Select
      </h2>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-6 max-w-4xl">
        <BaseSelect
          v-model="selectedOption"
          :options="selectOptions"
          label="Choose option"
          placeholder="Select an option"
        />

        <BaseSelect
          v-model="selectedOption"
          :options="selectOptions"
          label="With Error"
          placeholder="Select an option"
          error="Please select an option"
        />

        <BaseSelect
          v-model="selectedOption"
          :options="selectOptions"
          label="Glass Variant"
          placeholder="Select an option"
          variant="glass"
        />
      </div>
    </section>

    <!-- Cards -->
    <section class="space-y-4">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white">
        Cards
      </h2>

      <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <BaseCard>
          <h3 class="text-lg font-semibold text-gray-900 dark:text-white mb-2">
            Default Card
          </h3>
          <p class="text-gray-600 dark:text-gray-400 text-sm">
            This is a default card with standard styling.
          </p>
        </BaseCard>

        <BaseCard variant="glass">
          <h3 class="text-lg font-semibold text-gray-900 mb-2">
            Glass Card
          </h3>
          <p class="text-gray-600 text-sm">
            This is a glass morphism card variant.
          </p>
        </BaseCard>

        <BaseCard padding="sm">
          <h3 class="text-lg font-semibold text-gray-900 dark:text-white mb-2">
            Small Padding
          </h3>
          <p class="text-gray-600 dark:text-gray-400 text-sm">
            Card with smaller padding.
          </p>
        </BaseCard>
      </div>
    </section>

    <!-- Alerts -->
    <section class="space-y-4">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white">
        Alerts
      </h2>

      <div class="space-y-4 max-w-4xl">
        <BaseAlert variant="info" title="Information">
          This is an informational message with some details.
        </BaseAlert>

        <BaseAlert variant="success" title="Success">
          Your changes have been saved successfully.
        </BaseAlert>

        <BaseAlert variant="warning" title="Warning">
          Please review the information before proceeding.
        </BaseAlert>

        <BaseAlert variant="error" title="Error">
          An error occurred while processing your request.
        </BaseAlert>

        <BaseAlert variant="error" dismissible glass>
          This is a dismissible glass alert.
        </BaseAlert>
      </div>
    </section>

    <!-- Links -->
    <section class="space-y-4">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white">
        Links
      </h2>

      <div class="flex flex-wrap gap-6">
        <BaseLink to="/dashboard">
          Internal Link
        </BaseLink>

        <BaseLink href="https://example.com" external>
          External Link
        </BaseLink>

        <BaseLink to="/dashboard" variant="glass">
          Glass Variant Link
        </BaseLink>
      </div>
    </section>

    <!-- Divider -->
    <section class="space-y-4">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white">
        Dividers
      </h2>

      <div class="space-y-6 max-w-4xl">
        <div>
          <p class="text-gray-600 dark:text-gray-400 text-sm mb-2">
            Plain divider:
          </p>
          <BaseDivider />
        </div>

        <div>
          <p class="text-gray-600 dark:text-gray-400 text-sm mb-2">
            With text:
          </p>
          <BaseDivider text="or" />
        </div>

        <div>
          <p class="text-gray-600 dark:text-gray-400 text-sm mb-2">
            Glass variant:
          </p>
          <BaseDivider text="or" variant="glass" />
        </div>
      </div>
    </section>

    <!-- Dialog -->
    <section class="space-y-4">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white">
        Dialog
      </h2>

      <div class="flex gap-4">
        <BaseDialog v-model:open="dialogOpen" title="Example Dialog" description="This is a dialog built with Reka UI">
          <template #trigger>
            <BaseButton>Open Dialog</BaseButton>
          </template>

          <div class="space-y-4">
            <p class="text-gray-600 dark:text-gray-400">
              This is the dialog content. You can put any content here including forms, text, or other components.
            </p>

            <BaseInput
              v-model="email"
              type="email"
              label="Email"
              placeholder="Enter your email"
            />
          </div>

          <template #footer>
            <BaseButton variant="outline" @click="dialogOpen = false">
              Cancel
            </BaseButton>
            <BaseButton @click="dialogOpen = false">
              Save Changes
            </BaseButton>
          </template>
        </BaseDialog>
      </div>
    </section>

    <!-- Dropdown -->
    <section class="space-y-4">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white">
        Dropdown
      </h2>

      <div class="flex gap-4">
        <BaseDropdown :items="dropdownItems">
          <template #trigger>
            <BaseButton variant="outline">
              Actions
              <Icon name="lucide:chevron-down" class="w-4 h-4 ml-2" />
            </BaseButton>
          </template>
        </BaseDropdown>
      </div>
    </section>

    <!-- Tooltip -->
    <section class="space-y-4">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white">
        Tooltips
      </h2>

      <div class="flex gap-4">
        <BaseTooltip content="This is a tooltip">
          <BaseButton variant="outline">
            Hover me (Top)
          </BaseButton>
        </BaseTooltip>

        <BaseTooltip content="Tooltip on the right" side="right">
          <BaseButton variant="outline">
            Hover me (Right)
          </BaseButton>
        </BaseTooltip>

        <BaseTooltip side="bottom">
          <BaseButton variant="outline">
            Custom Content (Bottom)
          </BaseButton>
          <template #content>
            <div class="space-y-1">
              <p class="font-semibold">
                Custom Tooltip
              </p>
              <p class="text-xs opacity-90">
                With multiple lines
              </p>
            </div>
          </template>
        </BaseTooltip>
      </div>
    </section>
  </div>
</template>
