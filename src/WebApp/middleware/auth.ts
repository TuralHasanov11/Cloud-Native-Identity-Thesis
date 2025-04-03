export default defineNuxtRouteMiddleware((to, from) => {
  const { loggedIn } = useUserSession();

  if (!loggedIn.value) {
    console.log("User not authenticated, redirecting to login page");
    console.log(to, from);
    return navigateTo("/");
  }
});
