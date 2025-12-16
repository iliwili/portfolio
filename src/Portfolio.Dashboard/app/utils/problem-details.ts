export type ProblemDetails = {
  type?: string
  title?: string
  status?: number
  detail?: string
  instance?: string
  // Some serializers put extra props directly on the object.
  [key: string]: any
}

export type ValidationProblemDetails = ProblemDetails & {
  errors?: Record<string, string[]>
  // Sometimes extensions are nested, sometimes flattened.
  extensions?: Record<string, any>
}

export type FieldErrorEntry = { code: string, args?: any }
export type FieldErrorMap = Record<string, FieldErrorEntry[]>

function isObject(x: unknown): x is Record<string, any> {
  return !!x && typeof x === 'object'
}

function looksLikeProblemDetails(x: unknown): x is ProblemDetails {
  if (!isObject(x)) return false
  // Heuristics: at least one of these typically exists
  return (
    typeof x.title === 'string'
    || typeof x.detail === 'string'
    || typeof x.type === 'string'
    || typeof x.instance === 'string'
    || typeof x.status === 'number'
    || isObject((x as any).errors)
  )
}

/**
 * Extract the server payload from:
 * - axios error: e.response.data
 * - NSwag ApiException: e.result (common)
 * - NSwag ApiException: e.response (raw response text which is actually parsed JSON from axios)
 * - sometimes: e.data / e.body
 * - fallback: the error itself
 */
export function extractProblem(e: any): ProblemDetails | null {
  // Try common locations
  const payload
    = e?.response?.data
    ?? e?.result
    ?? e?.data
    ?? e?.body
    ?? null

  if (looksLikeProblemDetails(payload)) return payload

  // For NSwag generated clients, when status is unhandled (like 404),
  // the response property contains the parsed response data
  if (typeof e?.response === 'object' && e?.response !== null) {
    try {
      // If response is a string, try to parse it
      const parsed = typeof e.response === 'string' ? JSON.parse(e.response) : e.response
      if (looksLikeProblemDetails(parsed)) return parsed
    }
    catch {
      // Not JSON, continue
    }
  }

  if (looksLikeProblemDetails(e)) return e

  return null
}

/**
 * Get extensions regardless of whether they're nested under "extensions"
 * or flattened onto the object.
 */
export function getExtensions(problem: ProblemDetails | null): Record<string, any> {
  if (!problem) return {}
  const p: any = problem
  return (isObject(p.extensions) ? p.extensions : {}) as Record<string, any>
}

/**
 * Apply ValidationProblemDetails to a field-error accumulator.
 * Supports args in extensions.errorArgsByField[field][index].
 *
 * Returns true if field errors were applied; false if it's not a validation problem.
 */
export function applyProblemToErrors(
  problem: ValidationProblemDetails | null,
  add: (field: string, code: string, args?: any) => void,
): boolean {
  console.log('applyProblemToErrors', problem)
  if (!problem) return false

  const errors = problem.errors
  if (!errors || !isObject(errors)) return false

  const ext = getExtensions(problem)
  const argsByField = (ext.errorArgsByField ?? {}) as Record<string, any>

  for (const [field, codes] of Object.entries(errors)) {
    const list = Array.isArray(codes) ? codes : []
    const argsArr = Array.isArray(argsByField[field]) ? argsByField[field] : []

    for (let i = 0; i < list.length; i++) {
      const code = list[i]
      if (!code) continue
      add(field, code, argsArr[i])
    }
  }

  return true
}

/**
 * Extract a single "general" error code/args (banner-level) from ProblemDetails.
 * Looks for extensions.errorCode/errorArgs or flattened errorCode/errorArgs.
 */
export function getGeneralError(problem: ProblemDetails | null): { code?: string, args?: any, title?: string, detail?: string } {
  if (!problem) return {}

  console.log(problem)

  const ext = getExtensions(problem)
  const p: any = problem

  const code = ext.errorCode ?? p.errorCode
  const args = ext.errorArgs ?? p.errorArgs

  return {
    code: typeof code === 'string' ? code : undefined,
    args,
    title: typeof p.title === 'string' ? p.title : undefined,
    detail: typeof p.detail === 'string' ? p.detail : undefined,
  }
}

/**
 * Convenience: convert a ProblemDetails payload into a FieldErrorMap.
 * Useful if you want to just set/merge errors in one go.
 */
export function problemToFieldErrorMap(problem: ValidationProblemDetails | null): FieldErrorMap | null {
  const map: FieldErrorMap = {}

  const ok = applyProblemToErrors(problem, (field, code, args) => {
    (map[field] ??= []).push({ code, args })
  })

  return ok ? map : null
}
