export type PaginationResponse<TEntity, TCursor> = {
  pageCursor: TCursor
  pageSize: number
  count: number
  data: TEntity[]
}
