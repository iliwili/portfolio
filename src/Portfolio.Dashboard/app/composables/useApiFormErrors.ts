import { ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { extractProblem, getGeneralError } from '~/utils/problem-details'

export function useProblemFormErrors(addFromProblem: (eOrProblem: any) => void) {
  const { t } = useI18n()
  const formError = ref('')

  function clearFormError() {
    formError.value = ''
  }

  function applyError(e: any) {
    const problem = extractProblem(e)
    console.log(problem)

    // Try field errors first (ValidationProblemDetails)
    addFromProblem(problem ?? e)

    // If field errors were present, no need to set banner.
    // We detect that by checking if it's a validation problem.
    if (problem?.errors) return

    console.log('tester', problem)
    // Otherwise: general ProblemDetails
    const g = getGeneralError(problem)
    console.log(g)
    if (g.code) {
      formError.value = t(g.code, g.args ?? {})
      return
    }

    // fallback
    formError.value = problem?.title ?? t('common.error')
  }

  return { formError, clearFormError, applyError }
}
