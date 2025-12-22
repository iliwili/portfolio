import { z } from 'zod'

export const registerSchema = z.object({
  firstName: z.string()
    .min(1, 'First name is required')
    .max(50, 'First name must be less than 50 characters'),

  lastName: z.string()
    .min(1, 'Last name is required')
    .max(50, 'Last name must be less than 50 characters'),

  userName: z.string()
    .min(3, 'Username must be at least 3 characters')
    .max(30, 'Username must be less than 30 characters')
    .regex(/^[a-zA-Z0-9_-]+$/, 'Username can only contain letters, numbers, hyphens and underscores'),

  email: z.string()
    .min(1, 'Email is required')
    .email('Please enter a valid email address'),

  accountName: z.string()
    .min(3, 'Site name must be at least 3 characters')
    .max(50, 'Site name must be less than 50 characters')
    .regex(/^[a-zA-Z0-9\s-]+$/, 'Site name can only contain letters, numbers, spaces and hyphens'),

  slug: z.string()
    .min(3, 'Slug must be at least 3 characters')
    .max(50, 'Slug must be less than 50 characters')
    .regex(/^[a-z0-9-]+$/, 'Slug can only contain lowercase letters, numbers and hyphens'),

  password: z.string()
    .min(8, 'Password must be at least 8 characters')
    .regex(/[A-Z]/, 'Password must contain at least one uppercase letter')
    .regex(/[a-z]/, 'Password must contain at least one lowercase letter')
    .regex(/[0-9]/, 'Password must contain at least one number')
    .regex(/[^A-Za-z0-9]/, 'Password must contain at least one special character'),

  confirmPassword: z.string()
    .min(1, 'Please confirm your password'),
}).refine(data => data.password === data.confirmPassword, {
  message: 'Passwords do not match',
  path: ['confirmPassword'],
})

export type RegisterFormData = z.infer<typeof registerSchema>
