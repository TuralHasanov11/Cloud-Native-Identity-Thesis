import type { User } from "#auth-utils";

export default defineEventHandler(async (event) => {
  const authenticated = true;

  if(authenticated){
    const user: User = { name: "John Doe", email: "test@test.com", id: "123"};
    console.log("User authenticated:", user);
    await setUserSession(event, {
        user,
        loggedInAt: new Date().toISOString(),
    });
  }
});
