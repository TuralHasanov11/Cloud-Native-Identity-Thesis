export type User = {
  id: string
  name: string
  email: string

  roles?: string[]
  address: Address
}

export type Address = {
  street: string
  city: string
  state: string
  country: string
  zipCode: string
}

//   export type Role = {
//     id: string;
//     name: string;
//   };

//   export enum Roles {
//     Student = 'Student',
//     Instructor = 'Instructor',
//     Administrator = 'Administrator',
//     Editor = 'Editor',
//   }
