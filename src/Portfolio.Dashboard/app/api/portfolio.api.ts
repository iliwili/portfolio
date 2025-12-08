import type { AxiosInstance } from 'axios';
import axios from 'axios';
import { replace } from 'lodash';

export function client(apiUrl: string) {
  const result = axios.create({
    baseURL: apiUrl,
    withCredentials: true,
  })

  return result
}

class ApiFactory {
  create<T>(Type: new (url: string, client: AxiosInstance) => T): T {
    const config = useRuntimeConfig()
    const url = replace(config.public.baseUrl, '/api/', '')
    return new Type(url, client(url))
  }
}

export const apiFactory = new ApiFactory()

export default apiFactory