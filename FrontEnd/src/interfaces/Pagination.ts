export interface ItemPagination<T> {
  items: T[];
  totalItems: number;
  page: number;
  pageSize: number;
}
