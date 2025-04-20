import type { RouteMeta } from 'vue-router'

export type CustomRouteMeta = RouteMeta & {
  requireAuth?: boolean
  requireRole?: string[]
}

export enum HttpStatusCode {
  OK = 200,
  Created = 201,
  Accepted = 202,
  NoContent = 204,
  BadRequest = 400,
  Unauthorized = 401,
  Forbidden = 403,
  NotFound = 404,
  MethodNotAllowed = 405,
  Conflict = 409,
  InternalServerError = 500,
  NotImplemented = 501,
  BadGateway = 502,
  ServiceUnavailable = 503,
  GatewayTimeout = 504,
}
