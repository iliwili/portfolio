import { computed, reactive } from 'vue'
import type { ZodType, z } from 'zod'
import { useI18n } from 'vue-i18n'
import { applyProblemToErrors, type ValidationProblemDetails, extractProblem } from '~/utils/problem-details'

type FieldError = { code: string, args?: any }
export type ErrorMap = Record<string, FieldError[]>

function zodIssuesToErrorMap(issues: z.ZodIssue[]): ErrorMap {
  const map: ErrorMap = {}

  for (const issue of issues) {
    // ["lines", 1, "amount"] -> "lines[1].amount"
    const key = issue.path.reduce<string>((acc, seg) => {
      if (typeof seg === 'number') return `${acc}[${seg}]`
      return acc ? `${acc}.${String(seg)}` : String(seg)
    }, '')

    const code
      = typeof issue.message === 'string' && issue.message.length
        ? issue.message
        : 'validation.invalid'

      ; (map[key] ??= []).push({ code })
  }

  return map
}

export function useValidate<TValues>(schema: ZodType<TValues>) {
  const { t } = useI18n()
  const errors = reactive<ErrorMap>({})

  function clearErrors() {
    for (const k of Object.keys(errors)) Reflect.deleteProperty(errors, k)
  }

  function setErrors(map: ErrorMap) {
    clearErrors()
    for (const [k, v] of Object.entries(map)) errors[k] = v
  }

  function add(field: string, code: string, args?: any) {
    ; (errors[field] ??= []).push({ code, args })
  }

  function validate(values: unknown): values is TValues {
    clearErrors()
    const result = schema.safeParse(values)
    if (result.success) return true

    setErrors(zodIssuesToErrorMap(result.error.issues))
    return false
  }

  /**
   * Add server-side field errors from ValidationProblemDetails (400).
   * Accepts either:
   * - already-extracted problem payload
   * - or the thrown exception (it will extract internally)
   */
  function addFromProblem(problemOrException: ValidationProblemDetails | any) {
    const problem: any
      = problemOrException?.errors ? problemOrException : extractProblem(problemOrException)

    console.log('addFromProblem', problem)
    applyProblemToErrors(problem, (field, code, args) => add(field, code, args))
  }

  function errorText(field: string) {
    const first = errors[field]?.[0]
    return first ? t(first.code, first.args ?? {}) : ''
  }

  const hasErrors = computed(() => Object.keys(errors).length > 0)

  return {
    errors,
    hasErrors,
    validate, // Zod validation
    add, // manual add if needed
    addFromProblem, // server ValidationProblemDetails -> errors map
    clearErrors,
    errorText,
  }
}
