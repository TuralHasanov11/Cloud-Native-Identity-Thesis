export const formatDate = (value: string) => {
  const date = new Date(value)
  return date.toLocaleDateString('eu-DE', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  })
}
