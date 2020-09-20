export const MONTH_LABELS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

function pad(value: number): string {
  return String(value).padStart(2, '0');
}

/** yyyy-MM, as used by <input type="month"> */
export function toMonthValue(year: number, month: number): string {
  return `${year}-${pad(month)}`;
}

/** yyyy-MM-dd in local time, as used by <input type="date"> */
export function toDateValue(date: Date): string {
  return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}`;
}
