import { joinURL } from "ufo";

export default defineEventHandler(async (event) => {
    const proxyUrl = useRuntimeConfig().bffProxyUrl as string;

    const target = joinURL(proxyUrl, event.path);
    console.log("target", target); 
    try {
        // Proxy the request to the target server
        const response = await $fetch(target);
        console.log("response", response);
        return;

    } catch (error: any) {
        console.error("Error proxying request:", error);
        throw createError({
            statusCode: 502,
            statusMessage: "Bad Gateway",
            message: error.message || "Failed to proxy request",
        });
    }
})
