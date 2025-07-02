export type User = {
  id: string
  name: string
  email: string

  roles?: string[]
  groups?: string[]
  address: Address
}

export type Address = {
  street: string
  city: string
  state: string
  country: string
  zipCode: string
}

export enum MicrosoftEntraIdRoles {
  Admin = 'Admin',
}

export enum AwsCognitoGroups {
  Admin = 'Admin',
}
